namespace TmEssentials;

/// <summary>
/// Represents a time interval as a 32-bit integer, with <paramref name="TotalMilliseconds"/> as the unit.
/// This struct offers accuracy and high performance for scenarios where time precision to the millisecond is sufficient - driven Trackmania times for example.
/// Operators for comparing and arithmetic operations are included.
/// </summary>
/// <param name="TotalMilliseconds">The total number of milliseconds.</param>
public readonly record struct TimeInt32(int TotalMilliseconds) : ITime
{
    /// <summary>
    /// Represents a zero time interval.
    /// </summary>
    public static readonly TimeInt32 Zero = new();

    /// <summary>
    /// Represents the maximum time interval that can be represented by this struct.
    /// </summary>
    public static readonly TimeInt32 MaxValue = new(int.MaxValue);

    /// <summary>
    /// Represents the minimum time interval that can be represented by this struct.
    /// </summary>
    public static readonly TimeInt32 MinValue = new(int.MinValue);

    /// <inheritdoc />
    public int Days => (int)TotalDays;
    /// <inheritdoc />
    public int Hours => (int)TotalHours % 24;
    /// <inheritdoc />
    public int Milliseconds => TotalMilliseconds % 1000;
    /// <inheritdoc />
    public int Minutes => (int)TotalMinutes % 60;
    /// <inheritdoc />
    public int Seconds => (int)TotalSeconds % 60;
    /// <inheritdoc />
    public long Ticks => (long)TotalMilliseconds * 10_000;
    /// <inheritdoc />
    public float TotalDays => TotalMilliseconds / 86_400_000f;
    /// <inheritdoc />
    public float TotalHours => TotalMilliseconds / 3_600_000f;
    /// <inheritdoc />
    public float TotalMinutes => TotalMilliseconds / 60_000f;
    /// <inheritdoc />
    public float TotalSeconds => TotalMilliseconds / 1_000f;
    /// <inheritdoc />
    public bool IsNegative => TotalMilliseconds < 0;

    float ITime.TotalMilliseconds => TotalMilliseconds;

    /// <summary>
    /// Constructs a <see cref="TimeInt32"/> instance from hours, minutes, and seconds.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <param name="minutes">The number of minutes.</param>
    /// <param name="seconds">The number of seconds.</param>
    public TimeInt32(int hours, int minutes, int seconds) : this(0, hours, minutes, seconds)
    {

    }

    /// <summary>
    /// Constructs a <see cref="TimeInt32"/> instance from days, hours, minutes, seconds, and optional milliseconds.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <param name="hours">The number of hours.</param>
    /// <param name="minutes">The number of minutes.</param>
    /// <param name="seconds">The number of seconds.</param>
    /// <param name="milliseconds">The number of milliseconds (optional).</param>
    public TimeInt32(int days, int hours, int minutes, int seconds, int milliseconds = 0)
         : this(milliseconds + seconds * 1_000 + minutes * 60_000 + hours * 3_600_000 + days * 86_400_000)
    {

    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeInt32"/> to a Trackmania familiar time format.
    /// </summary>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    public string ToString(bool useHundredths = false, bool useApostrophe = false)
    {
        return TimeFormatter.ToTmString(Days, Hours, Minutes, Seconds, Milliseconds, TotalHours, TotalDays, IsNegative, useHundredths, useApostrophe);
    }

    /// <summary>
    /// Converts the value of the current <see cref="TimeInt32"/> to a Trackmania familiar time format with milliseconds and without apostrophes.
    /// </summary>
    /// <returns>A string representation of Trackmania time format.</returns>
    public override string ToString()
    {
        return ToString(useHundredths: false);
    }

    /// <summary>
    /// Creates a new <see cref="TimeInt32"/> instance representing a specified number of days.
    /// </summary>
    /// <param name="value">The number of days.</param>
    /// <returns>A <see cref="TimeInt32"/> instance equivalent to the specified number of days.</returns>
    public static TimeInt32 FromDays(float value) => new((int)(value * 86_400_000));

    /// <summary>
    /// Creates a new <see cref="TimeInt32"/> instance representing a specified number of hours.
    /// </summary>
    /// <param name="value">The number of hours.</param>
    /// <returns>A <see cref="TimeInt32"/> instance equivalent to the specified number of hours.</returns>
    public static TimeInt32 FromHours(float value) => new((int)(value * 3_600_000));

    /// <summary>
    /// Creates a new <see cref="TimeInt32"/> instance representing a specified number of milliseconds.
    /// </summary>
    /// <param name="value">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeInt32"/> instance equivalent to the specified number of milliseconds.</returns>
    public static TimeInt32 FromMilliseconds(float value) => new((int)value);

    /// <summary>
    /// Creates a new <see cref="TimeInt32"/> instance representing a specified number of minutes.
    /// </summary>
    /// <param name="value">The number of minutes.</param>
    /// <returns>A <see cref="TimeInt32"/> instance equivalent to the specified number of minutes.</returns>
    public static TimeInt32 FromMinutes(float value) => new((int)(value * 60_000));

    /// <summary>
    /// Creates a new <see cref="TimeInt32"/> instance representing a specified number of seconds.
    /// </summary>
    /// <param name="value">The number of seconds.</param>
    /// <returns>A <see cref="TimeInt32"/> instance equivalent to the specified number of seconds.</returns>
    public static TimeInt32 FromSeconds(float value) => new((int)(value * 1_000));

    /// <summary>
    /// Creates a new <see cref="TimeInt32"/> instance representing a specified number of ticks.
    /// </summary>
    /// <param name="value">The number of ticks (1 tick = 100 nanoseconds).</param>
    /// <returns>A <see cref="TimeInt32"/> instance equivalent to the specified number of ticks.</returns>
    public static TimeInt32 FromTicks(long value) => new((int)(value / 10_000));

    /// <inheritdoc />
    public ITime Add(ITime time)
    {
        return new TimeInt32(TotalMilliseconds + (int)time.TotalMilliseconds);
    }

    /// <inheritdoc />
    public ITime Divide(float divisor)
    {
        return this / divisor;
    }

    /// <inheritdoc />
    public float Divide(ITime time)
    {
        return TotalSeconds / time.TotalSeconds;
    }

    /// <inheritdoc />
    public ITime Duration()
    {
        return new TimeInt32(TotalMilliseconds >= 0 ? TotalMilliseconds : -TotalMilliseconds);
    }

    /// <inheritdoc />
    public ITime Multiply(float factor)
    {
        return this * factor;
    }

    /// <inheritdoc />
    public ITime Negate()
    {
        return -this;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public bool Equals(ITime? other)
    {
        if (other is null)
        {
            return false;
        }

        return TotalMilliseconds == other.TotalMilliseconds;
    }

    /// <summary>
    /// Converts this <see cref="TimeInt32"/> to its equal <see cref="TimeSingle"/> value.
    /// </summary>
    /// <returns>A <see cref="TimeSingle"/>.</returns>
    public TimeSingle ToTimeSingle() => new(TotalSeconds);

    /// <summary>
    /// Compares two <see cref="TimeInt32"/> instances to determine if the first is greater than the second.
    /// </summary>
    public static bool operator >(TimeInt32 t1, TimeInt32 t2) => t1.TotalMilliseconds > t2.TotalMilliseconds;

    /// <summary>
    /// Compares two <see cref="TimeInt32"/> instances to determine if the first is greater than or equal to the second.
    /// </summary>
    public static bool operator >=(TimeInt32 t1, TimeInt32 t2) => t1.TotalMilliseconds >= t2.TotalMilliseconds;

    /// <summary>
    /// Compares two <see cref="TimeInt32"/> instances to determine if the first is less than the second.
    /// </summary>
    public static bool operator <(TimeInt32 t1, TimeInt32 t2) => t1.TotalMilliseconds < t2.TotalMilliseconds;

    /// <summary>
    /// Compares two <see cref="TimeInt32"/> instances to determine if the first is less than or equal to the second.
    /// </summary>
    public static bool operator <=(TimeInt32 t1, TimeInt32 t2) => t1.TotalMilliseconds <= t2.TotalMilliseconds;

    /// <summary>
    /// Determines if a <see cref="TimeInt32"/> instance and a <see cref="TimeSingle"/> instance represent the same time interval.
    /// </summary>
    public static bool operator ==(TimeInt32 t1, TimeSingle t2) => t1.TotalMilliseconds == t2.TotalMilliseconds;

    /// <summary>
    /// Determines if a <see cref="TimeInt32"/> instance and a <see cref="TimeSingle"/> instance do not represent the same time interval.
    /// </summary>
    public static bool operator !=(TimeInt32 t1, TimeSingle t2) => t1.TotalMilliseconds != t2.TotalMilliseconds;

    /// <summary>
    /// Returns the same <see cref="TimeInt32"/> instance (identity operation).
    /// </summary>
    public static TimeInt32 operator +(TimeInt32 t) => t;

    /// <summary>
    /// Adds two <see cref="TimeInt32"/> instances together.
    /// </summary>
    public static TimeInt32 operator +(TimeInt32 t1, TimeInt32 t2) => new(t1.TotalMilliseconds + t2.TotalMilliseconds);

    /// <summary>
    /// Adds a <see cref="TimeSpan"/> to a <see cref="TimeInt32"/> instance.
    /// </summary>
    public static TimeInt32 operator +(TimeInt32 t1, TimeSpan t2) => new(t1.TotalMilliseconds + (int)t2.TotalMilliseconds);

    /// <summary>
    /// Negates a <see cref="TimeInt32"/> instance (reverses the sign of the interval).
    /// </summary>
    public static TimeInt32 operator -(TimeInt32 t) => new(-t.TotalMilliseconds);

    /// <summary>
    /// Subtracts one <see cref="TimeInt32"/> instance from another.
    /// </summary>
    public static TimeInt32 operator -(TimeInt32 t1, TimeInt32 t2) => new(t1.TotalMilliseconds - t2.TotalMilliseconds);

    /// <summary>
    /// Subtracts a <see cref="TimeSpan"/> from a <see cref="TimeInt32"/> instance.
    /// </summary>
    public static TimeInt32 operator -(TimeInt32 t1, TimeSpan t2) => new(t1.TotalMilliseconds - (int)t2.TotalMilliseconds);

    /// <summary>
    /// Multiplies a <see cref="TimeInt32"/> instance by a float factor, resulting in a <see cref="TimeSingle"/> instance.
    /// </summary>
    public static TimeSingle operator *(float factor, TimeInt32 t) => new(factor * t.TotalSeconds);

    /// <summary>
    /// Multiplies a <see cref="TimeInt32"/> instance by an integer factor.
    /// </summary>
    public static TimeInt32 operator *(int factor, TimeInt32 t) => new(factor * t.TotalMilliseconds);

    /// <summary>
    /// Multiplies a <see cref="TimeInt32"/> instance by a float factor, resulting in a <see cref="TimeSingle"/> instance.
    /// </summary>
    public static TimeSingle operator *(TimeInt32 t, float factor) => factor * t;

    /// <summary>
    /// Multiplies a <see cref="TimeInt32"/> instance by an integer factor.
    /// </summary>
    public static TimeInt32 operator *(TimeInt32 t, int factor) => factor * t;

    /// <summary>
    /// Divides a <see cref="TimeInt32"/> instance by a float divisor, resulting in a <see cref="TimeSingle"/> instance.
    /// </summary>
    public static TimeSingle operator /(TimeInt32 t, float divisor) => new(t.TotalSeconds / divisor);

    /// <summary>
    /// Divides one <see cref="TimeInt32"/> instance by another, resulting in a float.
    /// </summary>
    public static float operator /(TimeInt32 t1, TimeInt32 t2) => t1.TotalMilliseconds / (float)t2.TotalMilliseconds;

    /// <summary>
    /// Implicitly converts a <see cref="TimeInt32"/> instance to a <see cref="TimeSpan"/>.
    /// </summary>
    public static implicit operator TimeSpan(TimeInt32 t) => TimeSpan.FromMilliseconds(t.TotalMilliseconds);

    /// <summary>
    /// Implicitly converts a <see cref="TimeSpan"/> to a <see cref="TimeInt32"/> instance.
    /// </summary>
    public static implicit operator TimeInt32(TimeSpan t) => FromMilliseconds((float)t.TotalMilliseconds);

    /// <summary>
    /// Implicitly converts a <see cref="TimeSingle"/> to a <see cref="TimeInt32"/> instance.
    /// </summary>
    public static implicit operator TimeInt32(TimeSingle t) => FromSeconds(t.TotalSeconds);
}
