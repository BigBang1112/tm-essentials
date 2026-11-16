using System;
using System.ComponentModel;
using System.Globalization;
using TmEssentials.Converters;

namespace TmEssentials.Tests;

public class TimeInt32TypeConverterTests
{
    private readonly TimeInt32TypeConverter _converter = new();

    [Test]
    public async Task CanConvertFrom_String_ReturnsTrue()
    {
        var result = _converter.CanConvertFrom(null, typeof(string));
        
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertFrom_Int_ReturnsFalse()
    {
        var result = _converter.CanConvertFrom(null, typeof(int));
        
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task CanConvertTo_String_ReturnsTrue()
    {
        var result = _converter.CanConvertTo(null, typeof(string));
        
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertTo_Int_ReturnsFalse()
    {
        var result = _converter.CanConvertTo(null, typeof(int));
        
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task ConvertFrom_ValidTimeString_ReturnsTimeInt32()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1:23.456");
        
        await Assert.That(result).IsTypeOf<TimeInt32>();
        var timeInt32 = (TimeInt32)result!;
        await Assert.That(timeInt32.TotalMilliseconds).IsEqualTo(83456); // 1min 23sec 456ms = 83456ms
    }

    [Test]
    public async Task ConvertFrom_EmptyString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "");
        
        await Assert.That(result).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task ConvertFrom_WhitespaceString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "   ");
        
        await Assert.That(result).IsEqualTo(TimeInt32.Zero);
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
        
        await Assert.That(exception.Message).Contains("Unable to parse 'invalid' as TimeInt32");
    }

    [Test]
    public async Task ConvertFrom_NonString_CallsBase()
    {
        await Assert.That(() => _converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123)).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertTo_TimeInt32ToString_ReturnsFormattedString()
    {
        var timeInt32 = new TimeInt32(83456); // 1min 23sec 456ms
        
        var result = _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeInt32, typeof(string));
        
        await Assert.That(result).IsEqualTo("1:23.456");
    }

    [Test]
    public async Task ConvertTo_TimeInt32ToNonString_CallsBase()
    {
        var timeInt32 = new TimeInt32(1000);
        
        await Assert.That(() => _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeInt32, typeof(int))).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    [Arguments("0:00.000", 0)]
    [Arguments("0:01.000", 1000)]
    [Arguments("1:00.000", 60000)]
    [Arguments("0:00.001", 1)]
    [Arguments("2:34.567", 154567)]
    public async Task ConvertFrom_VariousValidFormats_ReturnsCorrectTimeInt32(string input, int expectedMilliseconds)
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);
        
        await Assert.That(result).IsTypeOf<TimeInt32>();
        var timeInt32 = (TimeInt32)result!;
        await Assert.That(timeInt32.TotalMilliseconds).IsEqualTo(expectedMilliseconds);
    }

    [Test]
    public async Task RoundTripConversion_PreservesValue()
    {
        var originalTime = new TimeInt32(12345);
        
        // Convert to string
        var stringValue = _converter.ConvertTo(null, CultureInfo.InvariantCulture, originalTime, typeof(string)) as string;
        await Assert.That(stringValue).IsNotNull();
        
        // Convert back to TimeInt32
        var roundTripTime = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, stringValue);
        
        await Assert.That(roundTripTime).IsEqualTo(originalTime);
    }
}