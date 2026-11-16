
namespace TmEssentials.Tests;

public class TimeSingleExtensionsTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task ToTmString_CallsToString(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeSingle(234567890).ToString(useHundredths, useApostrophe);

        var actual = new TimeSingle(234567890).ToTmString(useHundredths, useApostrophe);

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task ToTmString_CallsToString_NullableWithValue(bool useHundredths, bool useApostrophe)
    {
        var expected = new TimeSingle(234567890).ToString(useHundredths, useApostrophe);

        var actual = new TimeSingle?(new(234567890)).ToTmString(useHundredths, useApostrophe);

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false, "-:--.---")]
    [Arguments(true, false, "-:--.--")]
    [Arguments(false, true, "-'--''---")]
    [Arguments(true, true, "-'--''--")]
    public async Task ToTmString_CallsToString_NullableWithoutValue(bool useHundredths, bool useApostrophe, string expected)
    {
        var actual = new TimeSingle?().ToTmString(useHundredths, useApostrophe);

        await Assert.That(actual).IsEqualTo(expected);
    }
}
