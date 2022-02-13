using System;
using System.Collections.Generic;
using System.Text;

namespace TmEssentials;

public static class TimeSpanExtensions
{
    public static int ToMilliseconds(this TimeSpan timeSpan)
    {
        return (int)timeSpan.TotalMilliseconds;
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format.
    /// </summary>
    /// <param name="timeSpan">A TimeSpan.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSpan timeSpan, bool useHundredths = false)
    {
        var milliseconds = timeSpan.Milliseconds;
        var seconds = timeSpan.Seconds;
        var minutes = timeSpan.Minutes;
        var hours = timeSpan.Hours;
        var days = timeSpan.Days;
        var totalHours = timeSpan.TotalHours;
        var totalDays = timeSpan.TotalDays;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var length = 7;
#endif

        var negative = timeSpan.Ticks < 0;

        if (negative)
        {
            milliseconds = -milliseconds;
            seconds = -seconds;
            minutes = -minutes;
            hours = -hours;
            days = -days;
            totalHours = -totalHours;
            totalDays = -totalDays;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            length++;
#endif
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var msPush = useHundredths ? 0 : 1;
        length += msPush;

        if (hours > 0)
        {
            length += 3;

            if (hours > 10)
            {
                length++;
            }
        }
        else if (minutes >= 10)
        {
            length++;
        }

        if (days > 0)
        {
            if (hours == 0)
            {
                length += 3;
            }

            if (days < 10) length += 2;
            else if (days < 100) length += 3;
            else if (days < 1000) length += 4;
            else if (days < 10000) length += 5;
            else if (days < 100000) length += 6;
            else if (days < 1000000) length += 7;
            else if (days < 10000000) length += 8;
            else if (days < 100000000) length += 9;
            else if (days < 1000000000) length += 10;
        }
#endif

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var span = new char[length].AsSpan();

        WriteNumberAndPush(ref span, offset: 0, milliseconds, expectedLength: 3, msOffset: useHundredths ? 1 : 0);

        var msLength = useHundredths ? 2 : 3;

        span[length - msLength - 1] = '.';

        WriteNumberAndPush(ref span, offset: msLength + 1, seconds, expectedLength: 2);

        span[length - msLength - 4] = ':';

        WriteNumberAndPush(ref span, offset: msLength + 4, minutes, expectedLength: hours > 0 ? 2 : 1);

        if (hours > 0 || days > 0)
        {
            span[length - msLength - 7] = ':';

            WriteNumberAndPush(ref span, offset: msLength + 7, hours, expectedLength: days > 0 ? 2 : 1);

            if (days > 0)
            {
                span[length - msLength - 10] = ':';

                days.TryFormat(span, out int daysCharsWritten);

                if (negative)
                {
                    for (var i = 0; i < daysCharsWritten; i++)
                    {
                        span[daysCharsWritten - i] = span[daysCharsWritten - 1 - i];
                    }
                }
            }
        }

        if (negative)
        {
            span[0] = '-';
        }

        return new string(span);
#else
		var formatBuilder = new StringBuilder();

		formatBuilder.Append(minutes);

		formatBuilder.Append(':');

		if (seconds < 10)
			formatBuilder.Append(0);
		formatBuilder.Append(seconds);

		formatBuilder.Append('.');

		if (milliseconds < 100)
			formatBuilder.Append(0);
		if (milliseconds < 10)
			formatBuilder.Append(0);
		formatBuilder.Append(milliseconds);

		if (useHundredths)
			formatBuilder.Remove(formatBuilder.Length - 1, 1);

		if (totalHours >= 1)
		{
			if (minutes < 10)
				formatBuilder.Insert(0, '0');

			formatBuilder.Insert(0, ':');
			formatBuilder.Insert(0, hours);
		}

		if (totalDays >= 1)
		{
			if (hours < 10)
				formatBuilder.Insert(0, '0');

			formatBuilder.Insert(0, ':');
			formatBuilder.Insert(0, days);
		}

		if (timeSpan.Ticks < 0)
		{
			formatBuilder.Insert(0, '-');
		}

		return formatBuilder.ToString();
#endif
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private static void WriteNumberAndPush(ref Span<char> span, int offset, int number, int expectedLength, int msOffset = 0)
    {
        var destinationOffset = span.Length - offset - 1;

        if (number > 0)
        {
            number.TryFormat(span, out int charsWritten);

            charsWritten -= msOffset;

            for (var i = 0; i < charsWritten; i++)
            {
                span[destinationOffset - i] = span[charsWritten - 1 - i];
            }

            for (var i = 0; i < expectedLength - charsWritten; i++)
            {
                span[destinationOffset - i - charsWritten] = '0';
            }

            return;
        }

        for (var i = 0; i < expectedLength; i++)
        {
            span[destinationOffset - i] = '0';
        }
    }
#endif

    /// <summary>
    /// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format. If the value is null, <paramref name="nullString"/> will be used.
    /// </summary>
    /// <param name="timeSpan">A TimeSpan.</param>
    /// <param name="nullString">A string to use if <paramref name="timeSpan"/> is null.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSpan? timeSpan, string nullString, bool useHundredths = false)
    {
        if (timeSpan.HasValue)
            return ToTmString(timeSpan.Value, useHundredths);
        return nullString;
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format. If the value is null, -:--.--- will be used.
    /// </summary>
    /// <param name="timeSpan">A TimeSpan.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSpan? timeSpan, bool useHundredths = false)
    {
        return ToTmString(timeSpan, useHundredths ? "-:--.--" : "-:--.---", useHundredths);
    }
}
