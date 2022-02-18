namespace TmEssentials;

public readonly record struct TimeInt32(int TotalMilliseconds) : ITime
{
    public static readonly TimeInt32 Zero = new();
    public static readonly TimeInt32 MaxValue = new(int.MaxValue);
    public static readonly TimeInt32 MinValue = new(int.MinValue);

    public int Days => (int)TotalDays;
    public int Hours => (int)TotalHours % 24;
    public int Milliseconds => TotalMilliseconds % 1000;
    public int Minutes => (int)TotalMinutes % 60;
    public int Seconds => (int)TotalSeconds % 60;
    public long Ticks => (long)TotalMilliseconds * 10_000;
    public float TotalDays => TotalMilliseconds / 86_400_000f;
    public float TotalHours => TotalMilliseconds / 3_600_000f;
    public float TotalMinutes => TotalMilliseconds / 60_000f;
    public float TotalSeconds => TotalMilliseconds / 1_000f;
    public bool IsNegative => TotalMilliseconds < 0;

    float ITime.TotalMilliseconds => TotalMilliseconds;

    public TimeInt32(int hours, int minutes, int seconds) : this(0, hours, minutes, seconds)
    {

    }

    public TimeInt32(int days, int hours, int minutes, int seconds, int milliseconds = 0)
         : this(milliseconds + seconds * 1_000 + minutes * 60_000 + hours * 3_600_000 + days * 86_400_000)
    {

    }

    public string ToString(bool useHundredths)
    {
        return TimeFormatter.ToTmString(Days, Hours, Minutes, Seconds, Milliseconds, TotalHours, TotalDays, IsNegative, useHundredths);
    }

    public override string ToString()
    {
        return ToString(useHundredths: false);
    }

    public static TimeInt32 FromDays(float value) => new((int)(value * 86_400_000));
    public static TimeInt32 FromHours(float value) => new((int)(value * 3_600_000));
    public static TimeInt32 FromMilliseconds(float value) => new((int)(value));
    public static TimeInt32 FromMinutes(float value) => new((int)(value * 60_000));
    public static TimeInt32 FromSeconds(float value) => new((int)(value * 1_000));
    public static TimeInt32 FromTicks(long value) => new((int)(value / 10_000));

    public ITime Add(ITime time)
    {
        return new TimeInt32(TotalMilliseconds + (int)time.TotalMilliseconds);
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
        return new TimeInt32(TotalMilliseconds >= 0 ? TotalMilliseconds : -TotalMilliseconds);
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

        var milliseconds = TotalMilliseconds;
        var otherMilliseconds = other.TotalMilliseconds;

        if (milliseconds > otherMilliseconds)
        {
            return 1;
        }

        if (milliseconds < otherMilliseconds)
        {
            return -1;
        }

        return 0;
    }

    public TimeSingle ToTimeSingle() => new(TotalSeconds);

    public static bool operator >(TimeInt32 t1, TimeInt32 t2) => t1.TotalSeconds > t2.TotalSeconds;
    public static bool operator >=(TimeInt32 t1, TimeInt32 t2) => t1.TotalSeconds >= t2.TotalSeconds;
    public static bool operator <(TimeInt32 t1, TimeInt32 t2) => t1.TotalSeconds < t2.TotalSeconds;
    public static bool operator <=(TimeInt32 t1, TimeInt32 t2) => t1.TotalSeconds <= t2.TotalSeconds;

    public static TimeInt32 operator +(TimeInt32 t) => t;
    public static TimeInt32 operator +(TimeInt32 t1, TimeInt32 t2) => new(t1.TotalMilliseconds + t2.TotalMilliseconds);
    public static TimeInt32 operator -(TimeInt32 t1, TimeInt32 t2) => new(t1.TotalMilliseconds - t2.TotalMilliseconds);
    public static TimeInt32 operator -(TimeInt32 t) => new(-t.TotalMilliseconds);
    public static TimeSingle operator *(float factor, TimeInt32 t) => new(factor * t.TotalSeconds);
    public static TimeInt32 operator *(int factor, TimeInt32 t) => new(factor * t.TotalMilliseconds);
    public static TimeSingle operator *(TimeInt32 t, float factor) => factor * t;
    public static TimeInt32 operator *(TimeInt32 t, int factor) => factor * t;
    public static TimeSingle operator /(TimeInt32 t, float divisor) => new(t.TotalSeconds / divisor);
    public static float operator /(TimeInt32 t1, TimeInt32 t2) => t1.TotalMilliseconds / (float)t2.TotalMilliseconds;

    public static implicit operator TimeSpan(TimeInt32 t) => TimeSpan.FromMilliseconds(t.TotalMilliseconds);
    public static implicit operator TimeInt32(TimeSpan t) => FromMilliseconds((float)t.TotalMilliseconds);
}
