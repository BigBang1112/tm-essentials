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
    public long Ticks => (long)TotalMilliseconds * 10_000;
    public float TotalDays => TotalSeconds / 86_400f;
    public float TotalHours => TotalSeconds / 3_600f;
    public float TotalMilliseconds => TotalSeconds * 1000;
    public float TotalMinutes => TotalSeconds / 60f;
    public bool IsNegative => TotalSeconds < 0;

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
    public static TimeSingle FromTicks(long value) => new(value / 10_000 / 1_000f);

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

    /// <summary>
    /// Converts this <see cref="TimeSingle"/> to its equal <see cref="TimeInt32"/> value.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>.</returns>
    public TimeInt32 ToTimeInt32() => new((int)TotalMilliseconds);

    /// <summary>
    /// Converts the value of the current <see cref="TimeSingle"/> to a Trackmania familiar time format.
    /// </summary>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public string ToString(bool useHundredths = false, bool useApostrophe = false)
    {
        return TimeFormatter.ToTmString(Days, Hours, Minutes, Seconds, (int)Milliseconds, TotalHours, TotalDays, IsNegative, useHundredths, useApostrophe);
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeSingle"/> to a Trackmania familiar time format with milliseconds and without apostrophes.
    /// </summary>
    /// <returns>A string representation of Trackmania time format.</returns>
    public override string ToString()
    {
        return ToString(useHundredths: false);
    }

    public static bool operator >(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds > t2.TotalSeconds;
    public static bool operator >=(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds >= t2.TotalSeconds;
    public static bool operator <(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds < t2.TotalSeconds;
    public static bool operator <=(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds <= t2.TotalSeconds;
    public static bool operator ==(TimeSingle t1, TimeInt32 t2) => t1.TotalSeconds == t2.TotalSeconds;
    public static bool operator !=(TimeSingle t1, TimeInt32 t2) => t1.TotalSeconds != t2.TotalSeconds;

    public static TimeSingle operator +(TimeSingle t) => t;
    public static TimeSingle operator +(TimeSingle t1, TimeSingle t2) => new(t1.TotalSeconds + t2.TotalSeconds);
    public static TimeSingle operator +(TimeSingle t1, TimeSpan t2) => new(t1.TotalSeconds + (float)t2.TotalSeconds);

    public static TimeSingle operator -(TimeSingle t) => new(-t.TotalSeconds);
    public static TimeSingle operator -(TimeSingle t1, TimeSingle t2) => new(t1.TotalSeconds - t2.TotalSeconds);
    public static TimeSingle operator -(TimeSingle t1, TimeSpan t2) => new(t1.TotalSeconds - (float)t2.TotalSeconds);

    public static TimeSingle operator *(float factor, TimeSingle t) => new(factor * t.TotalSeconds);
    public static TimeSingle operator *(TimeSingle t, float factor) => factor * t;
    public static TimeSingle operator /(TimeSingle t, float divisor) => new(t.TotalSeconds / divisor);
    public static float operator /(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds / t2.TotalSeconds;

    public static implicit operator TimeSpan(TimeSingle t) => TimeSpan.FromSeconds(t.TotalSeconds);
    public static implicit operator TimeSingle(TimeSpan t) => FromSeconds((float)t.TotalSeconds);
    public static implicit operator TimeSingle(TimeInt32 t) => FromMilliseconds(t.TotalMilliseconds);
}
