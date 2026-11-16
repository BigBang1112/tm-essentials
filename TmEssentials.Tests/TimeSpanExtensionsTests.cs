using System;

namespace TmEssentials.Tests;

public class TimeSpanExtensionsTests
{
    [Test]
    public async Task ToMilliseconds_WithoutTicks()
    {
        var expected = 23456;

        var actual = new TimeSpan(0, 0, 0, 23, 456).ToMilliseconds();

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task ToMilliseconds_WithTicks()
    {
        var expected = 23456;

        var actual = new TimeSpan(234567890).ToMilliseconds();

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task ToTmString_CallsToString(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeInt32(234567890).ToString(useHundredths, useApostrophe);

        var actual = TimeSpan.FromMilliseconds(234567890).ToTmString(useHundredths, useApostrophe);

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task ToTmString_CallsToString_NullableWithValue(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeInt32(234567890).ToString(useHundredths, useApostrophe);

        var actual = new TimeSpan?(TimeSpan.FromMilliseconds(234567890)).ToTmString(useHundredths, useApostrophe);

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false, "-:--.---")]
    [Arguments(true, false, "-:--.--")]
    [Arguments(false, true, "-'--''---")]
    [Arguments(true, true, "-'--''--")]
    public async Task ToTmString_CallsToString_NullableWithoutValue(bool useHundredths, bool useApostrophe, string expected)
    {
        var actual = new TimeSpan?().ToTmString(useHundredths, useApostrophe);

        await Assert.That(actual).IsEqualTo(expected);
    }
}
