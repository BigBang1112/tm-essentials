namespace TmEssentials;

public readonly record struct TimeSingle(float TotalSeconds) : ITime
{
    public static readonly TimeSingle Zero = new();
    public static readonly TimeSingle MaxValue = new(float.MaxValue);
    public static readonly TimeSingle MinValue = new(float.MinValue);

    public int Days => (int)TotalDays;
    public int Hours => (int)TotalHours % 24;
    public float Milliseconds => TotalMilliseconds % 1000; // helps with float errors
    public int Minutes => (int)TotalMinutes % 60;
    public int Seconds => (int)TotalSeconds % 60;
    public long Ticks => (long)(TotalSeconds * 10_000_000);
    public float TotalDays => TotalSeconds / 86_400f;
    public float TotalHours => TotalSeconds / 3_600f;
    public float TotalMilliseconds => TotalSeconds * 1000;
    public float TotalMinutes => TotalSeconds / 60f;

    int ITime.Milliseconds => (int)Milliseconds;

    public TimeSingle(int hours, int minutes, int seconds) : this(0, hours, minutes, seconds)
    {

    }

    public TimeSingle(int days, int hours, int minutes, int seconds, float milliseconds = 0)
         : this(milliseconds / 1_000 + seconds + minutes * 60 + hours * 3_600 + days * 86_400)
    {

    }

    public static TimeSingle FromDays(float value) => new(value * 86_400);
    public static TimeSingle FromHours(float value) => new(value * 3_600);
    public static TimeSingle FromMilliseconds(float value) => new(value / 1_000);
    public static TimeSingle FromMinutes(float value) => new(value * 60);
    public static TimeSingle FromSeconds(float value) => new(value);
    public static TimeSingle FromTicks(long value) => new(value / 10_000_000f);

    public ITime Add(ITime time)
    {
        return new TimeSingle(TotalSeconds + time.TotalSeconds);
    }

    public ITime Divide(float divisor)
    {
        return this / divisor;
    }

    public float Divide(ITime time)
    {
        return TotalSeconds / time.TotalSeconds;
    }

    public ITime Duration()
    {
        return new TimeSingle(TotalSeconds >= 0 ? TotalSeconds : -TotalSeconds);
    }

    public ITime Multiply(float factor)
    {
        return this * factor;
    }

    public ITime Negate()
    {
        return -this;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not ITime time)
        {
            throw new ArgumentException("Value must be ITime.", nameof(obj));
        }

        return CompareTo(time);
    }

    public int CompareTo(ITime? other)
    {
        if (other is null)
        {
            return 1;
        }

        var seconds = TotalSeconds;
        var otherSeconds = other.TotalSeconds;

        if (seconds > otherSeconds)
        {
            return 1;
        }

        if (seconds < otherSeconds)
        {
            return -1;
        }

        return 0;
    }

    public TimeInt32 ToTimeInt32() => new((int)TotalMilliseconds);

    public static bool operator >(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds > t2.TotalSeconds;
    public static bool operator >=(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds >= t2.TotalSeconds;
    public static bool operator <(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds < t2.TotalSeconds;
    public static bool operator <=(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds <= t2.TotalSeconds;
    
    public static TimeSingle operator +(TimeSingle t) => t;
    public static TimeSingle operator +(TimeSingle t1, TimeSingle t2) => new(t1.TotalSeconds + t2.TotalSeconds);
    public static TimeSingle operator -(TimeSingle t1, TimeSingle t2) => new(t1.TotalSeconds - t2.TotalSeconds);
    public static TimeSingle operator -(TimeSingle t) => new(-t.TotalSeconds);
    public static TimeSingle operator *(float factor, TimeSingle t) => new(factor * t.TotalSeconds);
    public static TimeSingle operator *(TimeSingle t, float factor) => factor * t;
    public static TimeSingle operator /(TimeSingle t, float divisor) => new(t.TotalSeconds / divisor);
    public static float operator /(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds / t2.TotalSeconds;
}
