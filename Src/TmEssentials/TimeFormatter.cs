#if NETSTANDARD2_0 || NET462_OR_GREATER
using System.Text;
#endif

#if NET6_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace TmEssentials;

internal static class TimeFormatter
{
    public static readonly string[] TimeFormats =
    [
        @"FFFFFFF",
        @"s\.FFFFFFF",
        @"m\:ss\.FFFFFFF",
        @"h\:mm\:ss\.FFFFFFF",
        @"d\:hh\:mm\:ss\.FFFFFFF",
        @"\-FFFFFFF",
        @"\-s\.FFFFFFF",
        @"\-m\:ss\.FFFFFFF",
        @"\-h\:mm\:ss\.FFFFFFF",
        @"\-d\:hh\:mm\:ss\.FFFFFFF"
    ];

#if NET6_0_OR_GREATER
    [UnmanagedCallersOnly(EntryPoint = "totmstring")]
    public static nint ToTmString(uint time)
    {
        return Marshal.StringToCoTaskMemUTF8(new TimeInt32((int)time).ToString());
    }
#endif

    public static string ToTmString(int days,
                                    int hours,
                                    int minutes,
                                    int seconds,
                                    int milliseconds,
                                    float totalHours,
                                    float totalDays,
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
        if (isNegative)
        {
            totalHours = -totalHours;
            totalDays = -totalDays;
        }

        var formatBuilder = new StringBuilder();

        if (isNegative)
            formatBuilder.Append('-');

        if (useCompact)
        {
            formatBuilder.Append(seconds);
        }
        else
        {
            if (totalDays >= 1)
            {
                formatBuilder.Append(days);
                formatBuilder.Append(colonSeparator);
            }

            if (totalHours >= 1)
            {
                if (totalDays >= 1 && hours < 10)
                    formatBuilder.Append('0');
                formatBuilder.Append(hours);
                formatBuilder.Append(colonSeparator);
            }

            if (totalHours >= 1 && minutes < 10)
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
