using System.ComponentModel;
using System.Globalization;

namespace TmEssentials;

/// <summary>
/// Provides a type converter for <see cref="TimeSingle"/> to convert between string and TimeSingle values.
/// </summary>
public class TimeSingleTypeConverter : TypeConverter
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
            return TimeSingle.Zero;
        }

        if (TimeSingle.TryParse(stringValue, culture, out var result))
        {
            return result;
        }

        throw new FormatException($"Unable to parse '{stringValue}' as TimeSingle.");
    }

    /// <inheritdoc />
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is TimeSingle timeSingle)
        {
            return timeSingle.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}