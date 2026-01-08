using System;
using System.ComponentModel;
using System.Globalization;
using Xunit;

namespace TmEssentials.Tests;

public class TimeInt32TypeConverterTests
{
    private readonly TimeInt32TypeConverter _converter = new();

    [Fact]
    public void CanConvertFrom_String_ReturnsTrue()
    {
        var result = _converter.CanConvertFrom(null, typeof(string));
        
        Assert.True(result);
    }

    [Fact]
    public void CanConvertFrom_Int_ReturnsFalse()
    {
        var result = _converter.CanConvertFrom(null, typeof(int));
        
        Assert.False(result);
    }

    [Fact]
    public void CanConvertTo_String_ReturnsTrue()
    {
        var result = _converter.CanConvertTo(null, typeof(string));
        
        Assert.True(result);
    }

    [Fact]
    public void CanConvertTo_Int_ReturnsFalse()
    {
        var result = _converter.CanConvertTo(null, typeof(int));
        
        Assert.False(result);
    }

    [Fact]
    public void ConvertFrom_ValidTimeString_ReturnsTimeInt32()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "1:23.456");
        
        Assert.IsType<TimeInt32>(result);
        var timeInt32 = (TimeInt32)result!;
        Assert.Equal(83456, timeInt32.TotalMilliseconds); // 1min 23sec 456ms = 83456ms
    }

    [Fact]
    public void ConvertFrom_EmptyString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "");
        
        Assert.Equal(TimeInt32.Zero, result);
    }

    [Fact]
    public void ConvertFrom_WhitespaceString_ReturnsZero()
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, "   ");
        
        Assert.Equal(TimeInt32.Zero, result);
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
        
        Assert.Contains("Unable to parse 'invalid' as TimeInt32", exception.Message);
    }

    [Fact]
    public void ConvertFrom_NonString_CallsBase()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123));
    }

    [Fact]
    public void ConvertTo_TimeInt32ToString_ReturnsFormattedString()
    {
        var timeInt32 = new TimeInt32(83456); // 1min 23sec 456ms
        
        var result = _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeInt32, typeof(string));
        
        Assert.Equal("1:23.456", result);
    }

    [Fact]
    public void ConvertTo_TimeInt32ToNonString_CallsBase()
    {
        var timeInt32 = new TimeInt32(1000);
        
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertTo(null, CultureInfo.InvariantCulture, timeInt32, typeof(int)));
    }

    [Theory]
    [InlineData("0:00.000", 0)]
    [InlineData("0:01.000", 1000)]
    [InlineData("1:00.000", 60000)]
    [InlineData("0:00.001", 1)]
    [InlineData("2:34.567", 154567)]
    public void ConvertFrom_VariousValidFormats_ReturnsCorrectTimeInt32(string input, int expectedMilliseconds)
    {
        var result = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);
        
        Assert.IsType<TimeInt32>(result);
        var timeInt32 = (TimeInt32)result!;
        Assert.Equal(expectedMilliseconds, timeInt32.TotalMilliseconds);
    }

    [Fact]
    public void RoundTripConversion_PreservesValue()
    {
        var originalTime = new TimeInt32(12345);
        
        // Convert to string
        var stringValue = _converter.ConvertTo(null, CultureInfo.InvariantCulture, originalTime, typeof(string)) as string;
        Assert.NotNull(stringValue);
        
        // Convert back to TimeInt32
        var roundTripTime = _converter.ConvertFrom(null, CultureInfo.InvariantCulture, stringValue);
        
        Assert.Equal(originalTime, roundTripTime);
    }
}