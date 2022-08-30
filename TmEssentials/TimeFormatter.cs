#if NETSTANDARD2_0 || NET462_OR_GREATER
using System.Text;
#endif

namespace TmEssentials;

internal static class TimeFormatter
{
    internal static string ToTmString(int days,
                                      int hours,
                                      int minutes,
                                      int seconds,
                                      int milliseconds,
                                      float totalHours,
                                      float totalDays,
                                      bool isNegative,
                                      bool useHundredths = false,
                                      bool useApostrophe = false)
    {
        var colonSep = useApostrophe ? '\'' : ':';

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var length = 7 + (useApostrophe ? 1 : 0);
#endif
        if (isNegative)
        {
            milliseconds = -milliseconds;
            seconds = -seconds;
            minutes = -minutes;
            hours = -hours;
            days = -days;

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            length++;
#else
            totalHours = -totalHours;
            totalDays = -totalDays;
#endif
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var msPush = useHundredths ? 0 : 1;
        length += msPush;

        if (hours > 0)
        {
            length += 3;

            if (hours >= 10)
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
        Span<char> array = stackalloc char[length];

        WriteNumberAndPush(array, offset: 0, milliseconds, expectedLength: 3, msOffset: useHundredths ? 1 : 0);

        var msLength = useHundredths ? 2 : 3;

        array[length - msLength - 1] = useApostrophe ? '\'' : '.';

        if (useApostrophe)
        {
            msLength++;
            array[length - msLength - 1] = '\'';
        }

        WriteNumberAndPush(array, offset: msLength + 1, seconds, expectedLength: 2);

        array[length - msLength - 4] = colonSep;

        WriteNumberAndPush(array, offset: msLength + 4, minutes, expectedLength: hours > 0 ? 2 : 1);

        if (hours > 0 || days > 0)
        {
            array[length - msLength - 7] = colonSep;

            WriteNumberAndPush(array, offset: msLength + 7, hours, expectedLength: days > 0 ? 2 : 1);

            if (days > 0)
            {
                array[length - msLength - 10] = colonSep;

                days.TryFormat(array, out int daysCharsWritten);

                if (isNegative)
                {
                    for (var i = 0; i < daysCharsWritten; i++)
                    {
                        array[daysCharsWritten - i] = array[daysCharsWritten - 1 - i];
                    }
                }
            }
        }

        if (isNegative)
        {
            array[0] = '-';
        }

        return new string(array);
#else
		var formatBuilder = new StringBuilder();

		formatBuilder.Append(minutes);

		formatBuilder.Append(colonSep);

		if (seconds < 10)
			formatBuilder.Append(0);
		formatBuilder.Append(seconds);

		formatBuilder.Append(useApostrophe ? "''" : '.');

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

			formatBuilder.Insert(0, colonSep);
			formatBuilder.Insert(0, hours);
		}

		if (totalDays >= 1)
		{
			if (hours < 10)
				formatBuilder.Insert(0, '0');

			formatBuilder.Insert(0, colonSep);
			formatBuilder.Insert(0, days);
		}

		if (isNegative)
		{
			formatBuilder.Insert(0, '-');
		}

		return formatBuilder.ToString();
#endif
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private static void WriteNumberAndPush(Span<char> array, int offset, int number, int expectedLength, int msOffset = 0)
    {
        var destinationOffset = array.Length - offset - 1;

        if (number > 0)
        {
            number.TryFormat(array, out int charsWritten);

            charsWritten -= msOffset;

            for (var i = 0; i < charsWritten; i++)
            {
                array[destinationOffset - i] = array[charsWritten - 1 - i];
            }

            for (var i = 0; i < expectedLength - charsWritten; i++)
            {
                array[destinationOffset - i - charsWritten] = '0';
            }

            return;
        }

        for (var i = 0; i < expectedLength; i++)
        {
            array[destinationOffset - i] = '0';
        }
    }
#endif
}
