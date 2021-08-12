using System;
using System.Collections.Generic;
using System.Text;

namespace TmEssentials
{
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
			var formatBuilder = new StringBuilder();

			var milliseconds = timeSpan.Milliseconds;
			var seconds = timeSpan.Seconds;
			var minutes = timeSpan.Minutes;
			var hours = timeSpan.Hours;
			var days = timeSpan.Days;
			var totalHours = timeSpan.TotalHours;
			var totalDays = timeSpan.TotalDays;

			if (timeSpan.Ticks < 0)
			{
				milliseconds = -milliseconds;
				seconds = -seconds;
				minutes = -minutes;
				hours = -hours;
				days = -days;
				totalHours = -totalHours;
				totalDays = -totalDays;
			}

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
		}

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
}
