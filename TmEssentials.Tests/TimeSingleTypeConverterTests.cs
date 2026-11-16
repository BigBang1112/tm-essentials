using System;
using System.ComponentModel;
using System.Globalization;
using TmEssentials.Converters;

namespace TmEssentials.Tests;

public class TimeSingleTypeConverterTests
{
    private readonly TimeSingleTypeConverter _converter = new();

    [Test]
    public async Task CanConvertFrom_String_ReturnsTrue()
    {
        var result = _converter.CanConvertFrom(null, typeof(string));
        
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertFrom_Float_ReturnsFalse()
    {
        var result = _converter.CanConvertFrom(null, typeof(float));
        
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task CanConvertTo_String_ReturnsTrue()
    {
        var result = _converter.CanConvertTo(null, typeof(string));
        
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertTo_Float_ReturnsFalse()
    {
        var result = _converter.CanConvertTo(null, typeof(float));
        
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ConvertFrom_ValidTimeString_ReturnsTimeSingle()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1:23.456");
        
        await Assert.That(result).IsTypeOf<TimeSingle>();
        var timeSingle = (TimeSingle)result!;
        await Assert.That(timeSingle.TotalSeconds).IsEqualTo(83.456f).Within(0.001f);
    }

    [Test]
    public async Task ConvertFrom_EmptyString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "");
        
        await Assert.That(result).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task ConvertFrom_WhitespaceString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "   ");
        
        await Assert.That(result).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task ConvertFrom_NullString_CallsBase()
    {
        await Assert.That(() => _converter.ConvertFrom(null, CultureInfo.InvariantCulture, null)).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertFrom_InvalidString_ThrowsFormatException()
    {
        var exception = await Assert.That(() => _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "invalid")).ThrowsExactly<FormatException>();
        
        await Assert.That(exception.Message).Contains("Unable to parse 'invalid' as TimeSingle");
    }

    [Test]
    public async Task ConvertFrom_NonString_CallsBase()
    {
        await Assert.That(() => _converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123.45f)).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertTo_TimeSingleToString_ReturnsFormattedString()
    {
        var timeSingle = new TimeSingle(83.456f); // 1min 23.456sec
        
        var result = _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeSingle, typeof(string));
        
        await Assert.That(result).IsEqualTo("1:23.456");
    }

    [Test]
    public async Task ConvertTo_TimeSingleToNonString_CallsBase()
    {
        var timeSingle = new TimeSingle(1.0f);
        
        await Assert.That(() => _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeSingle, typeof(float))).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    [Arguments("0:00.000", 0.0f)]
    [Arguments("0:01.000", 1.0f)]
    [Arguments("1:00.000", 60.0f)]
    [Arguments("0:00.100", 0.1f)]
    [Arguments("2:34.567", 154.567f)]
    public async Task ConvertFrom_VariousValidFormats_ReturnsCorrectTimeSingle(string input, float expectedSeconds)
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);
        
        await Assert.That(result).IsTypeOf<TimeSingle>();
        var timeSingle = (TimeSingle)result!;
        await Assert.That(timeSingle.TotalSeconds).IsEqualTo(expectedSeconds).Within(0.001f);
    }

    [Test]
    public async Task RoundTripConversion_PreservesValue()
    {
        var originalTime = new TimeSingle(123.456f);
        
        // Convert to string
        var stringValue = _converter.ConvertTo(null, CultureInfo.InvariantCulture, originalTime, typeof(string)) as string;
        await Assert.That(stringValue).IsNotNull();
        
        // Convert back to TimeSingle
        var roundTripTime = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, stringValue);
        
        await Assert.That(roundTripTime).IsEqualTo(originalTime);
    }

    [Test]
    public async Task ConvertFrom_SubsecondPrecision_HandlesCorrectly()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "0:00.123");
        
        await Assert.That(result).IsTypeOf<TimeSingle>();
        var timeSingle = (TimeSingle)result!;
        await Assert.That(timeSingle.TotalSeconds).IsEqualTo(0.123f).Within(0.001f);
    }
}