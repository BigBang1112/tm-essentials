using System.ComponentModel;
using System.Globalization;

namespace TmEssentials;

/// <summary>
/// Provides a type converter for <see cref="TimeInt32"/> to convert between string and TimeInt32 values.
/// </summary>
public class TimeInt32TypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc />
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string stringValue)
        {
            return base.ConvertFrom(context, culture, value);
        }

        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return TimeInt32.Zero;
        }

        if (TimeInt32.TryParse(stringValue, culture, out var result))
        {
            return result;
        }

        throw new FormatException($"Unable to parse '{stringValue}' as TimeInt32.");
    }

    /// <inheritdoc />
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is TimeInt32 timeInt32)
        {
            return timeInt32.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}