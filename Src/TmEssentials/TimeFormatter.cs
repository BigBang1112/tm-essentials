#if NETSTANDARD2_0 || NET462_OR_GREATER
using System.Text;
#endif

namespace TmEssentials;

internal static class TimeFormatter
{
    /// <summary>
    /// The largest number of days that fits into <see cref="TimeSpan"/>.
    /// </summary>
    private const int MaxDays = 10_675_199;

    /// <summary>
    /// The largest number of whole milliseconds that fits into <see cref="TimeSpan"/>.
    /// </summary>
    private const long MaxMilliseconds = long.MaxValue / TimeSpan.TicksPerMillisecond;

    /// <summary>
    /// The number of fraction digits that <see cref="TimeSpan"/> ticks have (1 tick = 100 nanoseconds).
    /// </summary>
    private const int MaxFractionDigits = 7;

    private const int MaxDayDigits = 8;

    private static readonly int[] FractionScales = [10_000_000, 1_000_000, 100_000, 10_000, 1_000, 100, 10, 1];

    /// <summary>
    /// Parses a time in one of the formats <c>F</c>, <c>s.F</c>, <c>m:ss.F</c>, <c>h:mm:ss.F</c> and <c>d:hh:mm:ss.F</c>,
    /// optionally with a minus sign in front, where <c>F</c> is 0 to 7 digits of the fraction of a second.
    /// </summary>
    /// <remarks>
    /// The rules are the same as with <c>TimeSpan.TryParseExact</c> and the custom formats <c>FFFFFFF</c>, <c>s\.FFFFFFF</c>,
    /// <c>m\:ss\.FFFFFFF</c>, <c>h\:mm\:ss\.FFFFFFF</c> and <c>d\:hh\:mm\:ss\.FFFFFFF</c>, also with <c>\-</c> in front:
    /// <c>s</c>, <c>m</c> and <c>h</c> have 1 or 2 digits, <c>d</c> has 1 to 8 digits, the two-letter parts have exactly 2 digits,
    /// no whitespace is allowed, and the result must fit into <see cref="TimeSpan"/>. Only <c>TimeSpan.TryParseExact</c> is a lot slower.
    /// </remarks>
    /// <param name="s">The time.</param>
    /// <param name="result">The parsed time, or zero on failure.</param>
    /// <returns>True if the time was parsed, otherwise false.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out TimeSpan result)
    {
        result = default;

        if (s.IsEmpty)
        {
            return false;
        }

        // TimeSpan.TryParseExact with the format FFFFFFF reads any single character that is not a digit as zero,
        // because its tokenizer cannot step back before the first character. Kept for compatibility.
        if (s.Length == 1 && (uint)(s[0] - '0') > 9)
        {
            result = TimeSpan.Zero;
            return true;
        }

        var negative = s[0] == '-';
        var pos = negative ? 1 : 0;

        var firstLength = ReadNumber(s, ref pos, MaxDayDigits, out var first);

        if (firstLength < 0)
        {
            return false;
        }

        // Only the fraction of a second, like 5 for half a second (a minus sign alone is zero)
        if (pos == s.Length)
        {
            return firstLength <= MaxFractionDigits
                && TryCreateTimeSpan(0, 0, 0, 0, first, firstLength, negative, out result);
        }

        // Up to 3 more numbers of exactly 2 digits, separated by colons
        Span<int> numbers = stackalloc int[4];
        numbers[0] = first;
        var count = 1;

        while (s[pos] == ':')
        {
            pos++;

            if (count == numbers.Length || ReadNumber(s, ref pos, 2, out var number) != 2 || pos == s.Length)
            {
                return false;
            }

            numbers[count++] = number;
        }

        // Days have up to 8 digits, the first number of other formats has 1 or 2 digits
        if (s[pos] != '.' || firstLength == 0 || firstLength > (count == numbers.Length ? MaxDayDigits : 2))
        {
            return false;
        }

        pos++;

        var fractionLength = ReadNumber(s, ref pos, MaxFractionDigits, out var fraction);

        if (fractionLength < 0 || pos != s.Length)
        {
            return false;
        }

        return TryCreateTimeSpan(
            days: count == 4 ? numbers[count - 4] : 0,
            hours: count >= 3 ? numbers[count - 3] : 0,
            minutes: count >= 2 ? numbers[count - 2] : 0,
            seconds: numbers[count - 1],
            fraction,
            fractionLength,
            negative,
            out result);
    }

    /// <summary>
    /// Reads up to <paramref name="maxLength"/> ASCII digits.
    /// </summary>
    /// <returns>The number of digits read, or -1 if there are more than <paramref name="maxLength"/> digits.</returns>
    private static int ReadNumber(ReadOnlySpan<char> s, ref int pos, int maxLength, out int value)
    {
        value = 0;
        var start = pos;

        while (pos < s.Length && (uint)(s[pos] - '0') <= 9)
        {
            if (pos - start == maxLength)
            {
                return -1;
            }

            value = value * 10 + (s[pos] - '0');
            pos++;
        }

        return pos - start;
    }

    private static bool TryCreateTimeSpan(int days, int hours, int minutes, int seconds, int fraction, int fractionLength, bool negative, out TimeSpan result)
    {
        result = default;

        if (days > MaxDays || hours > 23 || minutes > 59 || seconds > 59)
        {
            return false;
        }

        var milliseconds = ((((long)days * 24 + hours) * 60 + minutes) * 60 + seconds) * 1000;

        if (milliseconds > MaxMilliseconds)
        {
            return false;
        }

        // The fraction digits are the leading digits of the 7 tick digits, so 5 is 5000000 ticks.
        // The sum can still overflow by the fraction, which TimeSpan.TryParseExact also rejects.
        var ticks = unchecked(milliseconds * TimeSpan.TicksPerMillisecond + (long)fraction * FractionScales[fractionLength]);

        if (ticks < 0)
        {
            return false;
        }

        result = new TimeSpan(negative ? -ticks : ticks);
        return true;
    }

    public static string ToTmString(int days,
                                    int hours,
                                    int minutes,
                                    int seconds,
                                    int milliseconds,
                                    bool isNegative,
                                    bool useHundredths = false,
                                    bool useApostrophe = false,
                                    bool compact = false)
    {
        var colonSeparator = useApostrophe ? '\'' : ':';

        if (isNegative)
        {
            milliseconds = -milliseconds;
            seconds = -seconds;
            minutes = -minutes;
            hours = -hours;
            days = -days;
        }

        if (useHundredths)
        {
            milliseconds /= 10;
        }

        // compact mode only applies when total time is under 1 minute
        var useCompact = compact && days == 0 && hours == 0 && minutes == 0;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        Span<char> buffer = stackalloc char[32];
        int pos = 0;

        if (isNegative)
            buffer[pos++] = '-';

        if (useCompact)
        {
            seconds.TryFormat(buffer.Slice(pos), out int secondCharsWritten);
            pos += secondCharsWritten;
        }
        else
        {
            if (days > 0)
            {
                days.TryFormat(buffer.Slice(pos), out int dayCharsWritten);
                pos += dayCharsWritten;
                buffer[pos++] = colonSeparator;
            }

            if (days > 0 || hours > 0)
            {
                if (days > 0 && hours < 10) buffer[pos++] = '0';
                hours.TryFormat(buffer.Slice(pos), out int hourCharsWritten);
                pos += hourCharsWritten;
                buffer[pos++] = colonSeparator;
            }

            if ((days > 0 || hours > 0) && minutes < 10)
                buffer[pos++] = '0';
            minutes.TryFormat(buffer.Slice(pos), out int minuteCharsWritten);
            pos += minuteCharsWritten;

            buffer[pos++] = colonSeparator;

            if (seconds < 10) buffer[pos++] = '0';
            seconds.TryFormat(buffer.Slice(pos), out int secondCharsWritten);
            pos += secondCharsWritten;
        }

        if (useApostrophe)
        {
            buffer[pos++] = '\'';
            buffer[pos++] = '\'';
        }
        else
        {
            buffer[pos++] = '.';
        }

        if (!useHundredths && milliseconds < 100) buffer[pos++] = '0';
        if (milliseconds < 10) buffer[pos++] = '0';
        milliseconds.TryFormat(buffer.Slice(pos), out int millisecondsWritten);
        pos += millisecondsWritten;

        return new string(buffer.Slice(0, pos));
#else
        var formatBuilder = new StringBuilder();

        if (isNegative)
            formatBuilder.Append('-');

        if (useCompact)
        {
            formatBuilder.Append(seconds);
        }
        else
        {
            if (days > 0)
            {
                formatBuilder.Append(days);
                formatBuilder.Append(colonSeparator);
            }

            if (days > 0 || hours > 0)
            {
                if (days > 0 && hours < 10)
                    formatBuilder.Append('0');
                formatBuilder.Append(hours);
                formatBuilder.Append(colonSeparator);
            }

            if ((days > 0 || hours > 0) && minutes < 10)
                formatBuilder.Append('0');
            formatBuilder.Append(minutes);

            formatBuilder.Append(colonSeparator);

            if (seconds < 10)
                formatBuilder.Append('0');
            formatBuilder.Append(seconds);
        }

        formatBuilder.Append(useApostrophe ? "''" : ".");

        if (!useHundredths && milliseconds < 100)
            formatBuilder.Append('0');
        if (milliseconds < 10)
            formatBuilder.Append('0');
        formatBuilder.Append(milliseconds);

        return formatBuilder.ToString();
#endif
    }
}
