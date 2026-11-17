using System.Globalization;
using TmEssentials.Converters;

namespace TmEssentials.Tests.Converters;

public class TimeInt32TypeConverterTests
{
    private readonly TimeInt32TypeConverter converter = new();

    [Test]
    public async Task CanConvertFrom_WithString_ReturnsTrue()
    {
        // Act
        var result = converter.CanConvertFrom(null, typeof(string));

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertFrom_WithInt32_ReturnsFalse()
    {
        // Act
        var result = converter.CanConvertFrom(null, typeof(int));

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task CanConvertTo_WithString_ReturnsTrue()
    {
        // Act
        var result = converter.CanConvertTo(null, typeof(string));

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertTo_WithInt32_ReturnsFalse()
    {
        // Act
        var result = converter.CanConvertTo(null, typeof(int));

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("0:00.000", 0)]
    [Arguments("0:00.001", 1)]
    [Arguments("0:01.000", 1_000)]
    [Arguments("1:00.000", 60_000)]
    [Arguments("1:23.456", 83_456)]
    [Arguments("2:34.567", 154_567)]
    public async Task ConvertFrom_WithTimeString_ReturnsTimeInt32(string input, int expectedMilliseconds)
    {
        // Act
        var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);

        // Assert
        await Assert.That(result).IsEqualTo(new TimeInt32(expectedMilliseconds));
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    public async Task ConvertFrom_WithBlankString_ReturnsZero(string input)
    {
        // Act
        var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);

        // Assert
        await Assert.That(result).IsEqualTo(TimeInt32.Zero);
    }

    [Test]
    public async Task ConvertFrom_WithInvalidString_ThrowsFormatExceptionNamingTheInput()
    {
        // Act
        var act = () => converter.ConvertFrom(null, CultureInfo.InvariantCulture, "invalid");

        // Assert
        var exception = await Assert.That(act).ThrowsExactly<FormatException>();
        await Assert.That(exception!.Message).Contains("Unable to parse 'invalid' as TimeInt32");
    }

    [Test]
    public async Task ConvertFrom_WithNull_ThrowsNotSupportedException()
    {
        // Act
        var act = () => converter.ConvertFrom(null, CultureInfo.InvariantCulture, null!);

        // Assert
        await Assert.That(act).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertFrom_WithNonString_ThrowsNotSupportedException()
    {
        // Act
        var act = () => converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123);

        // Assert
        await Assert.That(act).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertTo_WithStringType_ReturnsFormattedTime()
    {
        // Arrange
        var time = new TimeInt32(83_456);

        // Act
        var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, time, typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo("1:23.456");
    }

    [Test]
    public async Task ConvertTo_WithStringTypeAndNullValue_ReturnsEmptyString()
    {
        // Act
        var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, null, typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task ConvertTo_WithNonStringType_ThrowsNotSupportedException()
    {
        // Arrange
        var time = new TimeInt32(1_000);

        // Act
        var act = () => converter.ConvertTo(null, CultureInfo.InvariantCulture, time, typeof(int));

        // Assert
        await Assert.That(act).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertFrom_WithConvertToOutput_ReturnsOriginalTime()
    {
        // Arrange
        var original = new TimeInt32(12_345);
        var text = (string)converter.ConvertTo(null, CultureInfo.InvariantCulture, original, typeof(string))!;

        // Act
        var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, text);

        // Assert
        await Assert.That(result).IsEqualTo(original);
    }
}
