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
}
