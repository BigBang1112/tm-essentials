namespace TmEssentials;

public record struct TimeInt32(int TimeInMilliseconds) : ITime
{
    public static readonly TimeInt32 Zero = new();

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

    public TimeInt32(int hours, int minutes, int seconds) : this(0, hours, minutes, seconds)
    {

    }

    public TimeInt32(int days, int hours, int minutes, int seconds, int milliseconds = 0)
         : this(milliseconds + seconds * 1_000 + minutes * 60_000 + hours * 3_600_000 + days * 86_400_000)
    {
        
    }

    public static TimeInt32 FromDays(int value) => new(value * 86_400_000);
    public static TimeInt32 FromHours(int value) => new(value * 3_600_000);
    public static TimeInt32 FromMilliseconds(int value) => new(value);
    public static TimeInt32 FromMinutes(int value) => new(value * 60_000);
    public static TimeInt32 FromSeconds(int value) => new(value * 1_000);
    public static TimeInt32 FromTicks(long value) => new((int)(value / 10_000));

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

    public int CompareTo(TimeSpan other)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator +(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator /(TimeInt32 time, float divisor)
    {
        throw new NotImplementedException();
    }

    public static float operator /(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator >(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator >=(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator <(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static bool operator <=(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator *(float factor, TimeInt32 timeSpan)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator *(TimeInt32 timeSpan, float factor)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator -(TimeInt32 t1, TimeInt32 t2)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator -(TimeInt32 t)
    {
        throw new NotImplementedException();
    }

    public static TimeInt32 operator +(TimeInt32 t)
    {
        throw new NotImplementedException();
    }
}
