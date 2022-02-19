namespace TmEssentials;

public static class TimeInt32Extensions
{
    public static string ToTmString(this TimeInt32? time, string nullString, bool useHundredths = false)
    {
        if (time.HasValue)
        {
            return time.Value.ToString(useHundredths);
        }

        return nullString;
    }

    public static string ToTmString(this TimeSpan? time, bool useHundredths = false)
    {
        return ToTmString(time, useHundredths ? "-:--.--" : "-:--.---", useHundredths);
    }
}
