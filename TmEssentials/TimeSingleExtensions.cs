namespace TmEssentials;

public static class TimeSingleExtensions
{
    public static string ToTmString(this TimeSingle? time, string nullString, bool useHundredths = false)
    {
        if (time.HasValue)
        {
            return time.Value.ToString(useHundredths);
        }

        return nullString;
    }

    public static string ToTmString(this TimeSingle? time, bool useHundredths = false)
    {
        return ToTmString(time, useHundredths ? "-:--.--" : "-:--.---", useHundredths);
    }
}
