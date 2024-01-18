using System;
using Xunit;

namespace TmEssentials.Tests;

public class TimeSpanExtensionsTests
{
    [Fact]
    public void ToMilliseconds_WithoutTicks()
    {
        var expected = 23456;

        var actual = new TimeSpan(0, 0, 0, 23, 456).ToMilliseconds();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToMilliseconds_WithTicks()
    {
        var expected = 23456;

        var actual = new TimeSpan(234567890).ToMilliseconds();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void ToTmString_CallsToString(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeInt32(234567890).ToString(useHundredths, useApostrophe);

        var actual = TimeSpan.FromMilliseconds(234567890).ToTmString(useHundredths, useApostrophe);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void ToTmString_CallsToString_NullableWithValue(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeInt32(234567890).ToString(useHundredths, useApostrophe);

        var actual = new TimeSpan?(TimeSpan.FromMilliseconds(234567890)).ToTmString(useHundredths, useApostrophe);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(false, false, "-:--.---")]
    [InlineData(true, false, "-:--.--")]
    [InlineData(false, true, "-'--''---")]
    [InlineData(true, true, "-'--''--")]
    public void ToTmString_CallsToString_NullableWithoutValue(bool useHundredths, bool useApostrophe, string expected)
    {
        var actual = new TimeSpan?().ToTmString(useHundredths, useApostrophe);

        Assert.Equal(expected, actual);
    }
}
