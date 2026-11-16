namespace TmEssentials.Tests;

public class TimeFormatterTests
{
    [Test]
    [Arguments(0, 0, 0, 0, 3, "0:00.003")]
    [Arguments(0, 0, 0, 0, 33, "0:00.033")]
    [Arguments(0, 0, 0, 0, 333, "0:00.333")]
    [Arguments(0, 0, 0, 3, 333, "0:03.333")]
    [Arguments(0, 0, 3, 3, 333, "3:03.333")]
    [Arguments(0, 0, 33, 3, 333, "33:03.333")]
    [Arguments(0, 3, 33, 3, 333, "3:33:03.333")]
    [Arguments(1, 0, 56, 43, 165, "1:00:56:43.165")]
    [Arguments(1, 15, 56, 43, 165, "1:15:56:43.165")]
    [Arguments(0, 0, 0, 0, -333, "-0:00.333")]
    [Arguments(0, 0, 0, -3, -333, "-0:03.333")]
    [Arguments(0, 0, -3, -3, -333, "-3:03.333")]
    [Arguments(0, 0, -33, -3, -333, "-33:03.333")]
    [Arguments(0, -3, -33, -3, -333, "-3:33:03.333")]
    [Arguments(-1, -15, -56, -43, -165, "-1:15:56:43.165")]
    public async Task ToTmString_WithDefaultOptions_ReturnsTimeWithMilliseconds(int days, int hours, int minutes, int seconds, int milliseconds, string expected)
    {
        // Arrange
        var time = new TimeInt32(days, hours, minutes, seconds, milliseconds);

        // Act
        var actual = time.ToTmString();

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0, 0, 0, 333, "0:00.33")]
    [Arguments(0, 0, 0, 3, 333, "0:03.33")]
    [Arguments(0, 0, 3, 3, 333, "3:03.33")]
    [Arguments(0, 0, 33, 3, 333, "33:03.33")]
    [Arguments(0, 3, 33, 3, 333, "3:33:03.33")]
    [Arguments(1, 15, 56, 43, 165, "1:15:56:43.16")]
    [Arguments(0, 0, 0, 0, -333, "-0:00.33")]
    [Arguments(0, 0, 0, -3, -333, "-0:03.33")]
    [Arguments(0, 0, -3, -3, -333, "-3:03.33")]
    [Arguments(0, 0, -33, -3, -333, "-33:03.33")]
    [Arguments(0, -3, -33, -3, -333, "-3:33:03.33")]
    [Arguments(-1, -15, -56, -43, -165, "-1:15:56:43.16")]
    public async Task ToTmString_WithHundredths_TruncatesToHundredths(int days, int hours, int minutes, int seconds, int milliseconds, string expected)
    {
        // Arrange
        var time = new TimeInt32(days, hours, minutes, seconds, milliseconds);

        // Act
        var actual = time.ToTmString(useHundredths: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0, 0, 0, 333, false, "0'00''333")]
    [Arguments(0, 0, 0, 3, 333, false, "0'03''333")]
    [Arguments(0, 0, 3, 3, 333, false, "3'03''333")]
    [Arguments(0, 0, 33, 3, 333, false, "33'03''333")]
    [Arguments(0, 3, 33, 3, 333, false, "3'33'03''333")]
    [Arguments(1, 0, 56, 43, 165, false, "1'00'56'43''165")]
    [Arguments(1, 15, 56, 43, 165, false, "1'15'56'43''165")]
    [Arguments(0, 0, 0, 0, 333, true, "0'00''33")]
    [Arguments(0, 0, 0, 3, 333, true, "0'03''33")]
    [Arguments(0, 0, 3, 3, 333, true, "3'03''33")]
    [Arguments(0, 0, 33, 3, 333, true, "33'03''33")]
    [Arguments(0, 3, 33, 3, 333, true, "3'33'03''33")]
    [Arguments(1, 15, 56, 43, 165, true, "1'15'56'43''16")]
    [Arguments(0, 0, 0, 0, -333, false, "-0'00''333")]
    [Arguments(0, 0, 0, -3, -333, false, "-0'03''333")]
    [Arguments(0, 0, -3, -3, -333, false, "-3'03''333")]
    [Arguments(0, 0, -33, -3, -333, false, "-33'03''333")]
    [Arguments(0, -3, -33, -3, -333, false, "-3'33'03''333")]
    [Arguments(-1, -15, -56, -43, -165, false, "-1'15'56'43''165")]
    [Arguments(0, 0, 0, 0, -333, true, "-0'00''33")]
    [Arguments(0, 0, 0, -3, -333, true, "-0'03''33")]
    [Arguments(0, 0, -3, -3, -333, true, "-3'03''33")]
    [Arguments(0, 0, -33, -3, -333, true, "-33'03''33")]
    [Arguments(0, -3, -33, -3, -333, true, "-3'33'03''33")]
    [Arguments(-1, -15, -56, -43, -165, true, "-1'15'56'43''16")]
    public async Task ToTmString_WithApostrophe_UsesApostropheSeparators(int days, int hours, int minutes, int seconds, int milliseconds, bool useHundredths, string expected)
    {
        // Arrange
        var time = new TimeInt32(days, hours, minutes, seconds, milliseconds);

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1, "1:00:00:00.000")]
    [Arguments(10, "10:00:00:00.000")]
    [Arguments(100, "100:00:00:00.000")]
    [Arguments(1_000, "1000:00:00:00.000")]
    [Arguments(10_000, "10000:00:00:00.000")]
    [Arguments(100_000, "100000:00:00:00.000")]
    [Arguments(1_000_000, "1000000:00:00:00.000")]
    [Arguments(10_000_000, "10000000:00:00:00.000")]
    public async Task ToTmString_WithManyDays_ReturnsAllDayDigits(int days, string expected)
    {
        // Arrange
        var time = TimeSpan.FromDays(days);

        // Act
        var actual = time.ToTmString();

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(100, "100:00:00:00.000")]
    [Arguments(1_000, "1000:00:00:00.000")]
    public async Task ToTmString_WithManyDaysInTimeSingle_ReturnsAllDayDigits(int days, string expected)
    {
        // Arrange
        var time = new TimeSingle(days, 0, 0, 0);

        // Act
        var actual = time.ToTmString();

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 3, false, false, "0.003")]
    [Arguments(0, 33, false, false, "0.033")]
    [Arguments(0, 333, false, false, "0.333")]
    [Arguments(3, 333, false, false, "3.333")]
    [Arguments(59, 999, false, false, "59.999")]
    [Arguments(0, 333, true, false, "0.33")]
    [Arguments(3, 333, true, false, "3.33")]
    [Arguments(0, -333, false, false, "-0.333")]
    [Arguments(-3, -333, false, false, "-3.333")]
    [Arguments(0, -333, true, false, "-0.33")]
    [Arguments(-3, -333, true, false, "-3.33")]
    [Arguments(0, 333, false, true, "0''333")]
    [Arguments(3, 333, false, true, "3''333")]
    [Arguments(0, 333, true, true, "0''33")]
    [Arguments(3, 333, true, true, "3''33")]
    [Arguments(-3, -333, false, true, "-3''333")]
    public async Task ToTmString_CompactUnderOneMinute_OmitsMinutes(int seconds, int milliseconds, bool useHundredths, bool useApostrophe, string expected)
    {
        // Arrange
        var time = new TimeInt32(0, 0, 0, seconds, milliseconds);

        // Act
        var actual = time.ToTmString(useHundredths, useApostrophe, compact: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 3, 3, 333, "3:03.333")]
    [Arguments(3, 33, 3, 333, "3:33:03.333")]
    public async Task ToTmString_CompactFromOneMinute_ReturnsFullFormat(int hours, int minutes, int seconds, int milliseconds, string expected)
    {
        // Arrange
        var time = new TimeInt32(0, hours, minutes, seconds, milliseconds);

        // Act
        var actual = time.ToTmString(compact: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }
}
