namespace TmEssentials;

public record struct TimeSingle(float TimeInSeconds) : ITime
{
    public static readonly TimeSingle Zero = new();

    public int Days => throw new NotImplementedException();
    public int Hours => throw new NotImplementedException();
    public int Milliseconds => throw new NotImplementedException();
    public int Minutes => throw new NotImplementedException();
    public int Seconds => throw new NotImplementedException();
    public long Ticks => throw new NotImplementedException();
    public float TotalDays => throw new NotImplementedException();
    public float TotalHours => throw new NotImplementedException();
    public float TotalMilliseconds => throw new NotImplementedException();
    public float TotalMinutes => throw new NotImplementedException();
    public float TotalSeconds => throw new NotImplementedException();

    public TimeSingle(float hours, float minutes, float seconds) : this(0, hours, minutes, seconds)
    {

    }

    public TimeSingle(float days, float hours, float minutes, float seconds, float milliseconds = 0)
         : this(milliseconds / 1_000f + seconds + minutes * 60 + hours * 3_600 + days * 86_400)
    {

    }

    public static TimeSingle FromDays(float value) => new(value * 86_400);
    public static TimeSingle FromHours(float value) => new(value * 3_600);
    public static TimeSingle FromMilliseconds(float value) => new(value / 1_000);
    public static TimeSingle FromMinutes(float value) => new(value * 60);
    public static TimeSingle FromSeconds(float value) => new(value);
    public static TimeSingle FromTicks(long value) => new(value / 10_000_000f);

    public ITime Add(ITime ts)
    {
        throw new NotImplementedException();
    }

    public ITime Divide(float divisor)
    {
        throw new NotImplementedException();
    }

    public float Divide(ITime ts)
    {
        throw new NotImplementedException();
    }

    public ITime Duration()
    {
        throw new NotImplementedException();
    }

    public ITime Multiply(float factor)
    {
        throw new NotImplementedException();
    }

    public ITime Negate()
    {
        throw new NotImplementedException();
    }

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(ITime? other)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator +(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator /(TimeSingle time, float divisor)
    {
        throw new NotImplementedException();
    }

    public static float operator /(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator >(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator >=(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator <(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator <=(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator *(float factor, TimeSingle timeSpan)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator *(TimeSingle timeSpan, float factor)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator -(TimeSingle t1, TimeSingle t2)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator -(TimeSingle t)
    {
        throw new NotImplementedException();
    }

    public static TimeSingle operator +(TimeSingle t)
    {
        throw new NotImplementedException();
    }
}
