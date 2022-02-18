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
        return TimeFormatter.ToTmString(timeSpan.Days,
                                        timeSpan.Hours,
                                        timeSpan.Minutes,
                                        timeSpan.Seconds,
                                        timeSpan.Milliseconds,
                                        (float)timeSpan.TotalHours,
                                        (float)timeSpan.TotalDays,
                                        timeSpan.Ticks < 0,
                                        useHundredths);
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
