using System;
using System.ComponentModel;
using System.Globalization;
using Xunit;

namespace TmEssentials.Tests;

public class TimeSingleTypeConverterTests
{
    private readonly TimeSingleTypeConverter _converter = new();

    [Fact]
    public void CanConvertFrom_String_ReturnsTrue()
    {
        var result = _converter.CanConvertFrom(null, typeof(string));
        
        Assert.True(result);
    }

    [Fact]
    public void CanConvertFrom_Float_ReturnsFalse()
    {
        var result = _converter.CanConvertFrom(null, typeof(float));
        
        Assert.False(result);
    }

    [Fact]
    public void CanConvertTo_String_ReturnsTrue()
    {
        var result = _converter.CanConvertTo(null, typeof(string));
        
        Assert.True(result);
    }

    [Fact]
    public void CanConvertTo_Float_ReturnsFalse()
    {
        var result = _converter.CanConvertTo(null, typeof(float));
        
        Assert.False(result);
    }

    [Fact]
    public void ConvertFrom_ValidTimeString_ReturnsTimeSingle()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1:23.456");
        
        Assert.IsType<TimeSingle>(result);
        var timeSingle = (TimeSingle)result!;
        Assert.Equal(83.456f, timeSingle.TotalSeconds, precision: 3);
    }

    [Fact]
    public void ConvertFrom_EmptyString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "");
        
        Assert.Equal(TimeSingle.Zero, result);
    }

    [Fact]
    public void ConvertFrom_WhitespaceString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "   ");
        
        Assert.Equal(TimeSingle.Zero, result);
    }

    [Fact]
    public void ConvertFrom_NullString_CallsBase()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertFrom(null, CultureInfo.InvariantCulture, null));
    }

    [Fact]
    public void ConvertFrom_InvalidString_ThrowsFormatException()
    {
        var exception = Assert.Throws<FormatException>(() =>
            _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "invalid"));
        
        Assert.Contains("Unable to parse 'invalid' as TimeSingle", exception.Message);
    }

    [Fact]
    public void ConvertFrom_NonString_CallsBase()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123.45f));
    }

    [Fact]
    public void ConvertTo_TimeSingleToString_ReturnsFormattedString()
    {
        var timeSingle = new TimeSingle(83.456f); // 1min 23.456sec
        
        var result = _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeSingle, typeof(string));
        
        Assert.Equal("1:23.456", result);
    }

    [Fact]
    public void ConvertTo_TimeSingleToNonString_CallsBase()
    {
        var timeSingle = new TimeSingle(1.0f);
        
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeSingle, typeof(float)));
    }

    [Theory]
    [InlineData("0:00.000", 0.0f)]
    [InlineData("0:01.000", 1.0f)]
    [InlineData("1:00.000", 60.0f)]
    [InlineData("0:00.100", 0.1f)]
    [InlineData("2:34.567", 154.567f)]
    public void ConvertFrom_VariousValidFormats_ReturnsCorrectTimeSingle(string input, float expectedSeconds)
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);
        
        Assert.IsType<TimeSingle>(result);
        var timeSingle = (TimeSingle)result!;
        Assert.Equal(expectedSeconds, timeSingle.TotalSeconds, precision: 3);
    }

    [Fact]
    public void RoundTripConversion_PreservesValue()
    {
        var originalTime = new TimeSingle(123.456f);
        
        // Convert to string
        var stringValue = _converter.ConvertTo(null, CultureInfo.InvariantCulture, originalTime, typeof(string)) as string;
        Assert.NotNull(stringValue);
        
        // Convert back to TimeSingle
        var roundTripTime = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, stringValue);
        
        Assert.Equal(originalTime, roundTripTime);
    }

    [Fact]
    public void ConvertFrom_SubsecondPrecision_HandlesCorrectly()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "0:00.123");
        
        Assert.IsType<TimeSingle>(result);
        var timeSingle = (TimeSingle)result!;
        Assert.Equal(0.123f, timeSingle.TotalSeconds, precision: 3);
    }
}