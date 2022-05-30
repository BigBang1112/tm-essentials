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
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSpan timeSpan, bool useHundredths = false, bool useApostrophe = false)
    {
        return TimeFormatter.ToTmString(timeSpan.Days,
                                        timeSpan.Hours,
                                        timeSpan.Minutes,
                                        timeSpan.Seconds,
                                        timeSpan.Milliseconds,
                                        (float)timeSpan.TotalHours,
                                        (float)timeSpan.TotalDays,
                                        timeSpan.Ticks < 0,
                                        useHundredths,
                                        useApostrophe);
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format. If the value is null, <paramref name="nullString"/> will be used.
    /// </summary>
    /// <param name="timeSpan">A TimeSpan.</param>
    /// <param name="nullString">A string to use if <paramref name="timeSpan"/> is null.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSpan? timeSpan, string nullString, bool useHundredths = false, bool useApostrophe = false)
    {
        return timeSpan.HasValue
            ? ToTmString(timeSpan.Value, useHundredths, useApostrophe)
            : nullString;
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeSpan"/> to a Trackmania familiar time format. If the value is null, -:--.--- will be used, or -'--''--- when <paramref name="useApostrophe"/> is true.
    /// </summary>
    /// <param name="timeSpan">A TimeSpan.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSpan? timeSpan, bool useHundredths = false, bool useApostrophe = false)
    {
        var nullStr = useHundredths
            ? (useApostrophe ? "-'--''--" : "-:--.--")
            : (useApostrophe ? "-'--''---" : "-:--.---");
        return ToTmString(timeSpan, nullStr, useHundredths, useApostrophe);
    }
}
