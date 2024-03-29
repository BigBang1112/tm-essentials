﻿namespace TmEssentials;

/// <summary>
/// Provides extension methods for <see cref="TimeSingle"/> to enhance its functionality.
/// </summary>
public static class TimeSingleExtensions
{
    /// <summary>
    /// Converts the value of the current <see cref="TimeSingle"/> to a Trackmania familiar time format.
    /// </summary>
    /// <remarks>This method just calls <see cref="TimeSingle.ToString(bool, bool)"/> and exists only for consistency.</remarks>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSingle time, bool useHundredths = false, bool useApostrophe = false)
    {
        return time.ToString(useHundredths, useApostrophe);
    }
    
    /// <summary>
    /// Converts the value of the current <see cref="TimeSingle"/> to a Trackmania familiar time format. If the value is null, <paramref name="nullString"/> will be used.
    /// </summary>
    /// <param name="time">A TimeSpan.</param>
    /// <param name="nullString">A string to use if <paramref name="time"/> is null.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSingle? time, string nullString, bool useHundredths = false, bool useApostrophe = false)
    {
        if (time.HasValue)
        {
            return time.Value.ToString(useHundredths, useApostrophe);
        }

        return nullString;
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeSingle"/> to a Trackmania familiar time format. If the value is null, -:--.--- will be used, or -'--''--- when <paramref name="useApostrophe"/> is true.
    /// </summary>
    /// <param name="time">A TimeSingle.</param>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public static string ToTmString(this TimeSingle? time, bool useHundredths = false, bool useApostrophe = false)
    {
        var nullStr = useHundredths
            ? (useApostrophe ? "-'--''--" : "-:--.--")
            : (useApostrophe ? "-'--''---" : "-:--.---");
        return ToTmString(time, nullStr, useHundredths, useApostrophe);
    }
}
