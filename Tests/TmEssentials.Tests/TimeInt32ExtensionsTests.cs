namespace TmEssentials.Tests;

public class TimeInt32ExtensionsTests
{
    [Test]
    [Arguments(false, false, "2:17:09:27.890")]
    [Arguments(true, false, "2:17:09:27.89")]
    [Arguments(false, true, "2'17'09'27''890")]
    [Arguments(true, true, "2'17'09'27''89")]
    public async Task ToTmString_WithValue_ReturnsFormattedTime(bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        var time = new TimeInt32(234_567_890);

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false, false, "2:17:09:27.890")]
    [Arguments(true, false, "2:17:09:27.89")]
    [Arguments(false, true, "2'17'09'27''890")]
    [Arguments(true, true, "2'17'09'27''89")]
    public async Task ToTmString_WithNullableWithValue_ReturnsFormattedTime(bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        TimeInt32? time = new TimeInt32(234_567_890);

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
        TimeInt32? time = null;

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }
}
