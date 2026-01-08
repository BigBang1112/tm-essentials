#if NET5_0_OR_GREATER || NET21_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.ComponentModel;

namespace TmEssentials;

/// <summary>
/// Represents a time interval as a 32-bit floating-point number, with <paramref name="TotalSeconds"/> as the unit.
/// This struct offers accuracy and high performance for scenarios - time of keyframes in MediaTracker for example.
/// Operators for comparing and arithmetic operations are included.
/// </summary>
/// <param name="TotalSeconds">The total number of seconds.</param>
[TypeConverter(typeof(TimeSingleTypeConverter))]
public readonly record struct TimeSingle(float TotalSeconds) : ITime,
    IComparable<TimeSingle>, IComparable<TimeInt32>, IEquatable<TimeInt32>
#if NET7_0_OR_GREATER
    , IParsable<TimeSingle>, ISpanParsable<TimeSingle>
#endif
{
    /// <summary>
    /// Represents a zero time interval.
    /// </summary>
    public static readonly TimeSingle Zero = new();

    /// <summary>
    /// Represents the maximum time interval that can be represented by this struct.
    /// </summary>
    public static readonly TimeSingle MaxValue = new(float.MaxValue);

    /// <summary>
    /// Represents the minimum time interval that can be represented by this struct.
    /// </summary>
    public static readonly TimeSingle MinValue = new(float.MinValue);

    /// <inheritdoc />
    public int Days => (int)TotalDays;
    /// <inheritdoc />
    public int Hours => (int)TotalHours % 24;
    /// <inheritdoc />
    public float Milliseconds => TotalMilliseconds % 1000; // helps with float errors
    /// <inheritdoc />
    public int Minutes => (int)TotalMinutes % 60;
    /// <inheritdoc />
    public int Seconds => (int)TotalSeconds % 60;
    /// <inheritdoc />
    public long Ticks => (long)TotalMilliseconds * 10_000;
    /// <inheritdoc />
    public float TotalDays => TotalSeconds / 86_400f;
    /// <inheritdoc />
    public float TotalHours => TotalSeconds / 3_600f;
    /// <inheritdoc />
    public float TotalMilliseconds => TotalSeconds * 1000;
    /// <inheritdoc />
    public float TotalMinutes => TotalSeconds / 60f;
    /// <inheritdoc />
    public bool IsNegative => TotalSeconds < 0;

    int ITime.Milliseconds => (int)Milliseconds;

    /// <summary>
    /// Constructs a <see cref="TimeSingle"/> instance from hours, minutes, and seconds.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <param name="minutes">The number of minutes.</param>
    /// <param name="seconds">The number of seconds.</param>
    public TimeSingle(int hours, int minutes, int seconds) : this(0, hours, minutes, seconds)
    {
    }

    /// <summary>
    /// Constructs a <see cref="TimeSingle"/> instance from days, hours, minutes, seconds, and optional milliseconds.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <param name="hours">The number of hours.</param>
    /// <param name="minutes">The number of minutes.</param>
    /// <param name="seconds">The number of seconds.</param>
    /// <param name="milliseconds">The number of milliseconds (optional).</param>
    public TimeSingle(int days, int hours, int minutes, int seconds, float milliseconds = 0)
         : this(milliseconds / 1_000 + seconds + minutes * 60 + hours * 3_600 + days * 86_400)
    {
    }

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

    /// <summary>
    /// Creates a new <see cref="TimeSingle"/> instance representing a specified number of days.
    /// </summary>
    /// <param name="value">The number of days.</param>
    /// <returns>A <see cref="TimeSingle"/> instance equivalent to the specified number of days.</returns>
    public static TimeSingle FromDays(float value) => new(value * 86_400);

    /// <summary>
    /// Creates a new <see cref="TimeSingle"/> instance representing a specified number of hours.
    /// </summary>
    /// <param name="value">The number of hours.</param>
    /// <returns>A <see cref="TimeSingle"/> instance equivalent to the specified number of hours.</returns>
    public static TimeSingle FromHours(float value) => new(value * 3_600);

    /// <summary>
    /// Creates a new <see cref="TimeSingle"/> instance representing a specified number of milliseconds.
    /// </summary>
    /// <param name="value">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSingle"/> instance equivalent to the specified number of milliseconds.</returns>
    public static TimeSingle FromMilliseconds(float value) => new(value / 1_000);

    /// <summary>
    /// Creates a new <see cref="TimeSingle"/> instance representing a specified number of minutes.
    /// </summary>
    /// <param name="value">The number of minutes.</param>
    /// <returns>A <see cref="TimeSingle"/> instance equivalent to the specified number of minutes.</returns>
    public static TimeSingle FromMinutes(float value) => new(value * 60);

    /// <summary>
    /// Creates a new <see cref="TimeSingle"/> instance representing a specified number of seconds.
    /// </summary>
    /// <param name="value">The number of seconds.</param>
    /// <returns>A <see cref="TimeSingle"/> instance equivalent to the specified number of seconds.</returns>
    public static TimeSingle FromSeconds(float value) => new(value);

    /// <summary>
    /// Creates a new <see cref="TimeSingle"/> instance representing a specified number of ticks.
    /// </summary>
    /// <param name="value">The number of ticks (1 tick = 100 nanoseconds).</param>
    /// <returns>A <see cref="TimeSingle"/> instance equivalent to the specified number of ticks.</returns>
    public static TimeSingle FromTicks(long value) => new(value / 10_000 / 1_000f);
    
    /// <inheritdoc />
    public ITime Add(ITime time)
    {
        return new TimeSingle(TotalSeconds + time.TotalSeconds);
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
        return new TimeSingle(TotalSeconds >= 0 ? TotalSeconds : -TotalSeconds);
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

        return TotalSeconds.CompareTo(other.TotalSeconds);
    }

    /// <summary>
    /// Compares this instance to a specified <see cref="TimeSingle"/> instance.
    /// </summary>
    /// <param name="other">A <see cref="TimeSingle"/> instance to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="other"/>.</returns>
    public int CompareTo(TimeSingle other)
    {
        return TotalSeconds.CompareTo(other.TotalSeconds);
    }

    /// <summary>
    /// Compares this instance to a specified <see cref="TimeInt32"/> instance.
    /// </summary>
    /// <param name="other">A <see cref="TimeInt32"/> instance to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="other"/>.</returns>
    public int CompareTo(TimeInt32 other)
    {
        return TotalSeconds.CompareTo(other.TotalSeconds);
    }

    /// <inheritdoc />
    public bool Equals(ITime? other)
    {
        if (other is null)
        {
            return false;
        }

        return TotalSeconds == other.TotalSeconds;
    }

    /// <inheritdoc />
    public bool Equals(TimeInt32 other)
    {
        return TotalSeconds == other.TotalSeconds;
    }

    /// <summary>
    /// Converts this <see cref="TimeSingle"/> to its equal <see cref="TimeInt32"/> value.
    /// </summary>
    /// <returns>A <see cref="TimeInt32"/>.</returns>
    public TimeInt32 ToTimeInt32() => new((int)TotalMilliseconds);

    /// <inheritdoc />
    public static bool TryParse(
#if NET5_0_OR_GREATER || NET21_OR_GREATER
        [NotNullWhen(true)]
#endif
        string? s, IFormatProvider? provider,
#if NET5_0_OR_GREATER || NET21_OR_GREATER
        [MaybeNullWhen(false)] 
#endif
        out TimeSingle result)
    {
        if (TimeSpan.TryParseExact(s, TimeFormatter.TimeFormats, provider, out var timeSpan))
        {
            result = (TimeSingle)(s?.Length > 0 && s[0] == '-' ? -timeSpan : timeSpan);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Tries to parse a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/> or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string s, out TimeSingle result) => TryParse(s, null, out result);

    /// <inheritdoc />
    public static TimeSingle Parse(string s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException("Input string was not in a correct format.");
    }

    /// <summary>
    /// Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    public static TimeSingle Parse(string s) => Parse(s, null);

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
#if NET5_0_OR_GREATER || NET21_OR_GREATER
        [MaybeNullWhen(false)]
#endif
        out TimeSingle result)
    {
#if NET5_0_OR_GREATER || NET21_OR_GREATER
        if (TimeSpan.TryParseExact(s, TimeFormatter.TimeFormats, provider, out var timeSpan))
#else
        if (TimeSpan.TryParseExact(s.ToString(), TimeFormatter.TimeFormats, provider, out var timeSpan))
#endif
        {
            result = (TimeSingle)(s.Length > 0 && s[0] == '-' ? -timeSpan : timeSpan);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Tries to parse a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s,
#if NET5_0_OR_GREATER || NET21_OR_GREATER
        [MaybeNullWhen(false)]
#endif
        out TimeSingle result) => TryParse(s, null, out result);

    /// <inheritdoc />
    public static TimeSingle Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException("Input string was not in a correct format.");
    }

    /// <summary>
    /// Parses a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    public static TimeSingle Parse(ReadOnlySpan<char> s) => Parse(s, null);

    /// <summary>
    /// Compares two <see cref="TimeSingle"/> instances to determine if the first is greater than the second.
    /// </summary>
    public static bool operator >(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds > t2.TotalSeconds;

    /// <summary>
    /// Compares two <see cref="TimeSingle"/> instances to determine if the first is greater than or equal to the second.
    /// </summary>
    public static bool operator >=(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds >= t2.TotalSeconds;

    /// <summary>
    /// Compares two <see cref="TimeSingle"/> instances to determine if the first is less than the second.
    /// </summary>
    public static bool operator <(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds < t2.TotalSeconds;

    /// <summary>
    /// Compares two <see cref="TimeSingle"/> instances to determine if the first is less than or equal to the second.
    /// </summary>
    public static bool operator <=(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds <= t2.TotalSeconds;

    /// <summary>
    /// Determines if two <see cref="TimeSingle"/> instances have the same value.
    /// </summary>
    public static bool operator ==(TimeSingle t1, TimeInt32 t2) => t1.TotalSeconds == t2.TotalSeconds;

    /// <summary>
    /// Determines if two <see cref="TimeSingle"/> instances do not have the same value.
    /// </summary>
    public static bool operator !=(TimeSingle t1, TimeInt32 t2) => t1.TotalSeconds != t2.TotalSeconds;

    /// <summary>
    /// Returns the same <see cref="TimeSingle"/> instance.
    /// </summary>
    public static TimeSingle operator +(TimeSingle t) => t;

    /// <summary>
    /// Adds the TotalSeconds of two <see cref="TimeSingle"/> instances.
    /// </summary>
    public static TimeSingle operator +(TimeSingle t1, TimeSingle t2) => new(t1.TotalSeconds + t2.TotalSeconds);

    /// <summary>
    /// Adds the TotalSeconds of a <see cref="TimeSingle"/> instance and a <see cref="TimeSpan"/>.
    /// </summary>
    public static TimeSingle operator +(TimeSingle t1, TimeSpan t2) => new(t1.TotalSeconds + (float)t2.TotalSeconds);

    /// <summary>
    /// Negates the TotalSeconds of a <see cref="TimeSingle"/> instance.
    /// </summary>
    public static TimeSingle operator -(TimeSingle t) => new(-t.TotalSeconds);

    /// <summary>
    /// Subtracts the TotalSeconds of one <see cref="TimeSingle"/> instance from another.
    /// </summary>
    public static TimeSingle operator -(TimeSingle t1, TimeSingle t2) => new(t1.TotalSeconds - t2.TotalSeconds);

    /// <summary>
    /// Subtracts the TotalSeconds of a <see cref="TimeSpan"/> from a <see cref="TimeSingle"/> instance.
    /// </summary>
    public static TimeSingle operator -(TimeSingle t1, TimeSpan t2) => new(t1.TotalSeconds - (float)t2.TotalSeconds);

    /// <summary>
    /// Multiplies the TotalSeconds of a <see cref="TimeSingle"/> instance by a float factor.
    /// </summary>
    public static TimeSingle operator *(float factor, TimeSingle t) => new(factor * t.TotalSeconds);

    /// <summary>
    /// Multiplies the TotalSeconds of a <see cref="TimeSingle"/> instance by a float factor.
    /// </summary>
    public static TimeSingle operator *(TimeSingle t, float factor) => factor * t;

    /// <summary>
    /// Divides the TotalSeconds of a <see cref="TimeSingle"/> instance by a float divisor.
    /// </summary>
    public static TimeSingle operator /(TimeSingle t, float divisor) => new(t.TotalSeconds / divisor);

    /// <summary>
    /// Divides the TotalSeconds of one <see cref="TimeSingle"/> instance by another, returning a float result.
    /// </summary>
    public static float operator /(TimeSingle t1, TimeSingle t2) => t1.TotalSeconds / t2.TotalSeconds;

    /// <summary>
    /// Implicitly converts a <see cref="TimeSingle"/> to a <see cref="TimeSpan"/> based on TotalSeconds.
    /// </summary>
    public static implicit operator TimeSpan(TimeSingle t) => TimeSpan.FromSeconds(t.TotalSeconds);

    /// <summary>
    /// Implicitly converts a <see cref="TimeSpan"/> to a <see cref="TimeSingle"/> based on TotalSeconds.
    /// </summary>
    public static implicit operator TimeSingle(TimeSpan t) => FromSeconds((float)t.TotalSeconds);

    /// <summary>
    /// Implicitly converts a <see cref="TimeInt32"/> to a <see cref="TimeSingle"/> based on TotalMilliseconds.
    /// </summary>
    public static implicit operator TimeSingle(TimeInt32 t) => FromMilliseconds(t.TotalMilliseconds);
}
