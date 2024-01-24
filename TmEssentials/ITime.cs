namespace TmEssentials;

/// <summary>
/// Represents a time value and provides methods for time calculations and comparisons.
/// This interface allows for the manipulation and comparison of time values, 
/// including support for basic arithmetic operations and conversions.
/// </summary>
public interface ITime : IComparable, IComparable<ITime>, IEquatable<ITime>
{
    /// <summary>
    /// Gets the number of whole days.
    /// </summary>
    int Days { get; }

    /// <summary>
    /// Gets the number of whole hours.
    /// </summary>
    int Hours { get; }

    /// <summary>
    /// Gets the number of milliseconds.
    /// </summary>
    int Milliseconds { get; }

    /// <summary>
    /// Gets the number of whole minutes.
    /// </summary>
    int Minutes { get; }

    /// <summary>
    /// Gets the number of whole seconds.
    /// </summary>
    int Seconds { get; }

    /// <summary>
    /// Gets the number of ticks.
    /// </summary>
    long Ticks { get; }

    /// <summary>
    /// Gets the total number of days, as a floating-point value.
    /// </summary>
    float TotalDays { get; }

    /// <summary>
    /// Gets the total number of hours, as a floating-point value.
    /// </summary>
    float TotalHours { get; }

    /// <summary>
    /// Gets the total number of milliseconds, as a floating-point value.
    /// </summary>
    float TotalMilliseconds { get; }

    /// <summary>
    /// Gets the total number of minutes, as a floating-point value.
    /// </summary>
    float TotalMinutes { get; }

    /// <summary>
    /// Gets the total number of seconds, as a floating-point value.
    /// </summary>
    float TotalSeconds { get; }

    /// <summary>
    /// Indicates whether the time value is negative.
    /// </summary>
    bool IsNegative { get; }

    /// <summary>
    /// Adds the specified <see cref="ITime"/> to this instance.
    /// </summary>
    /// <param name="ts">An <see cref="ITime"/> instance to add.</param>
    /// <returns>A new <see cref="ITime"/> instance representing the sum.</returns>
    ITime Add(ITime ts);

    /// <summary>
    /// Divides this instance by the specified divisor.
    /// </summary>
    /// <param name="divisor">The divisor.</param>
    /// <returns>A new ITime instance representing the quotient.</returns>
    ITime Divide(float divisor);

    /// <summary>
    /// Divides this instance by the specified ITime value.
    /// </summary>
    /// <param name="ts">An <see cref="ITime"/> instance to divide by.</param>
    /// <returns>The quotient as a floating-point value.</returns>
    float Divide(ITime ts);

    /// <summary>
    /// Returns the absolute value of this instance.
    /// </summary>
    /// <returns>A new <see cref="ITime"/> instance representing the duration.</returns>
    ITime Duration();

    /// <summary>
    /// Multiplies this instance by the specified factor.
    /// </summary>
    /// <param name="factor">The factor to multiply by.</param>
    /// <returns>A new <see cref="ITime"/> instance representing the product.</returns>
    ITime Multiply(float factor);

    /// <summary>
    /// Negates this time value.
    /// </summary>
    /// <returns>A new <see cref="ITime"/> instance with the negated value.</returns>
    ITime Negate();

    /// <summary>
    /// Converts the value of the current <see cref="TimeInt32"/> to a Trackmania familiar time format.
    /// </summary>
    /// <param name="useHundredths">If to use the hundredths instead of milliseconds (for better looks on TMUF for example)</param>
    /// <param name="useApostrophe">If to use ' instead of a colon and '' instead of a dot (to resolve cases where colon is not allowed for example).</param>
    /// <returns>A string representation of Trackmania time format.</returns>
    string ToString(bool useHundredths = false, bool useApostrophe = false);
}
