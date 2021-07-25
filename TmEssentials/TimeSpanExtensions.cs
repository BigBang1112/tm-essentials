using System;
using System.Collections.Generic;
using System.Text;

namespace TmEssentials
{
	public static class TimeSpanExtensions
	{
		public static int ToMilliseconds(this TimeSpan timeSpan)
		{
			return Convert.ToInt32(timeSpan.TotalMilliseconds);
		}

		/// <summary>
		/// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format.
		/// </summary>
		/// <param name="timeSpan">A TimeSpan.</param>
		/// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
		/// <returns>A string representation of Trackmania time format.</returns>
		public static string ToStringTm(this TimeSpan timeSpan, bool useHundredths = false)
		{
			var formatBuilder = new StringBuilder("m':'ss'.'ff");

			if (!useHundredths)
				formatBuilder.Append('f');

			if (timeSpan.TotalHours >= 1)
				formatBuilder.Insert(0, "h':'m");

			if (timeSpan.TotalDays >= 1)
				formatBuilder.Insert(0, "d':'h");

			if (timeSpan.Ticks < 0)
				formatBuilder.Insert(0, "'-'");

			return timeSpan.ToString(formatBuilder.ToString());
		}

		/// <summary>
		/// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format. If the value is null, <paramref name="nullString"/> will be used.
		/// </summary>
		/// <param name="timeSpan">A TimeSpan.</param>
		/// <param name="nullString">A string to use if <paramref name="timeSpan"/> is null.</param>
		/// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
		/// <returns>A string representation of Trackmania time format.</returns>
		public static string ToStringTm(this TimeSpan? timeSpan, string nullString, bool useHundredths = false)
		{
			if (timeSpan.HasValue)
				return ToStringTm(timeSpan.Value, useHundredths);
			return nullString;
		}

		/// <summary>
		/// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format. If the value is null, -:--.--- will be used.
		/// </summary>
		/// <param name="timeSpan">A TimeSpan.</param>
		/// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
		/// <returns>A string representation of Trackmania time format.</returns>
		public static string ToStringTm(this TimeSpan? timeSpan, bool useHundredths = false)
		{
			return ToStringTm(timeSpan, "-:--.--" + (useHundredths ? "" : "-"), useHundredths);
		}
	}
}
