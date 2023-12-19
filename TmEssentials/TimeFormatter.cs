#if NETSTANDARD2_0 || NET462_OR_GREATER
using System.Text;
#endif

using System.Runtime.InteropServices;

namespace TmEssentials;

internal static class TimeFormatter
{
#if NET6_0_OR_GREATER
    [UnmanagedCallersOnly(EntryPoint = "totmstring")]
    internal static nint ToTmString(uint time)
    {
        return Marshal.StringToCoTaskMemUTF8(new TimeInt32((int)time).ToString());
    }
#endif

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
        var length = (useApostrophe ? 8 : 7) + (useHundredths ? 0 : 1);
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

        Span<char> array = stackalloc char[length];

        if (useHundredths)
        {
            milliseconds /= 10;
        }

        if (milliseconds < 10)
        {
            milliseconds.TryFormat(array.Slice(length - 1), out int msCharsWritten);
            array[length - 2] = '0';
            array[length - 3] = '0';
        }
        else if (milliseconds < 100)
        {
            milliseconds.TryFormat(array.Slice(length - 2), out int msCharsWritten);
            array[length - 3] = '0';
        }
        else
        {
            milliseconds.TryFormat(array.Slice(length - 3), out int msCharsWritten);
        }

        var msLength = useHundredths ? 2 : 3;

        array[length - msLength - 1] = useApostrophe ? '\'' : '.';

        if (useApostrophe)
        {
            msLength++;
            array[length - msLength - 1] = '\'';
        }

        if (seconds < 10)
        {
            seconds.TryFormat(array.Slice(length - msLength - 2), out int secondsCharsWritten);
            array[length - msLength - 3] = '0';
        }
        else
        {
            seconds.TryFormat(array.Slice(length - msLength - 3), out int secondsCharsWritten);
        }

        array[length - msLength - 4] = colonSep;

        minutes.TryFormat(array.Slice(length - msLength - (minutes < 10 ? 5 : 6)), out int minutesCharsWritten);

        if (hours > 0 && minutes < 10)
        {
            array[length - msLength - 6] = '0';
        }

        if (hours > 0 || days > 0)
        {
            array[length - msLength - 7] = colonSep;

            hours.TryFormat(array.Slice(length - msLength - (hours < 10 ? 8 : 9)), out int hoursCharsWritten);

            if (days > 0 && hours < 10)
            {
                array[length - msLength - 9] = '0';
            }

            if (days > 0)
            {
                array[length - msLength - 10] = colonSep;

                if (isNegative)
                {
                    days.TryFormat(array.Slice(1), out int daysCharsWritten);
                }
                else
                {
                    days.TryFormat(array, out int daysCharsWritten);
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
}
