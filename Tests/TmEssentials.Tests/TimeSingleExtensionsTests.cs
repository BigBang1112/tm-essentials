namespace TmEssentials.Tests;

public class TimeSingleExtensionsTests
{
    [Test]
    [Arguments(false, false, "1:02:03.500")]
    [Arguments(true, false, "1:02:03.50")]
    [Arguments(false, true, "1'02'03''500")]
    [Arguments(true, true, "1'02'03''50")]
    public async Task ToTmString_WithValue_ReturnsFormattedTime(bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        var time = new TimeSingle(3_723.5f);

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false, "1:02:03.500")]
    [Arguments(true, false, "1:02:03.50")]
    [Arguments(false, true, "1'02'03''500")]
    [Arguments(true, true, "1'02'03''50")]
    public async Task ToTmString_WithNullableWithValue_ReturnsFormattedTime(bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        TimeSingle? time = new TimeSingle(3_723.5f);

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false, "-:--.---")]
    [Arguments(true, false, "-:--.--")]
    [Arguments(false, true, "-'--''---")]
    [Arguments(true, true, "-'--''--")]
    public async Task ToTmString_WithNullableWithoutValue_ReturnsPlaceholder(bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        TimeSingle? time = null;

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }
}
