namespace TmEssentials.Tests;

public class TimeSpanExtensionsTests
{
    [Test]
    public async Task ToMilliseconds_WithWholeMilliseconds_ReturnsTotalMilliseconds()
    {
        // Arrange
        var timeSpan = new TimeSpan(0, 0, 0, 23, 456);

        // Act
        var milliseconds = timeSpan.ToMilliseconds();

        // Assert
        await Assert.That(milliseconds).IsEqualTo(23_456);
    }

    [Test]
    public async Task ToMilliseconds_WithSubMillisecondTicks_TruncatesToWholeMilliseconds()
    {
        // Arrange
        var timeSpan = new TimeSpan(234_567_890); // 23456.789 ms

        // Act
        var milliseconds = timeSpan.ToMilliseconds();

        // Assert
        await Assert.That(milliseconds).IsEqualTo(23_456);
    }

    [Test]
    [Arguments(false, false, "2:17:09:27.890")]
    [Arguments(true, false, "2:17:09:27.89")]
    [Arguments(false, true, "2'17'09'27''890")]
    [Arguments(true, true, "2'17'09'27''89")]
    public async Task ToTmString_WithValue_ReturnsFormattedTime(bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        var time = TimeSpan.FromMilliseconds(234_567_890);

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
        TimeSpan? time = TimeSpan.FromMilliseconds(234_567_890);

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
        TimeSpan? time = null;

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }
}
