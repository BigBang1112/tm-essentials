using System.Globalization;
using TmEssentials.Converters;

namespace TmEssentials.Tests.Converters;

public class TimeSingleTypeConverterTests
{
    private const float HalfMillisecond = 0.0005f;

    private readonly TimeSingleTypeConverter converter = new();

    [Test]
    public async Task CanConvertFrom_WithString_ReturnsTrue()
    {
        // Act
        var result = converter.CanConvertFrom(null, typeof(string));

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task CanConvertFrom_WithSingle_ReturnsFalse()
    {
        // Act
        var result = converter.CanConvertFrom(null, typeof(float));

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
    public async Task CanConvertTo_WithSingle_ReturnsFalse()
    {
        // Act
        var result = converter.CanConvertTo(null, typeof(float));

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("0:00.000", 0f)]
    [Arguments("0:00.100", 0.1f)]
    [Arguments("0:00.123", 0.123f)]
    [Arguments("0:01.000", 1f)]
    [Arguments("1:00.000", 60f)]
    [Arguments("1:23.456", 83.456f)]
    [Arguments("2:34.567", 154.567f)]
    public async Task ConvertFrom_WithTimeString_ReturnsTimeSingle(string input, float expectedSeconds)
    {
        // Act
        var result = (TimeSingle)converter.ConvertFrom(null, CultureInfo.InvariantCulture, input)!;

        // Assert
        await Assert.That(result.TotalSeconds).IsEqualTo(expectedSeconds).Within(HalfMillisecond);
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    public async Task ConvertFrom_WithBlankString_ReturnsZero(string input)
    {
        // Act
        var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, input);

        // Assert
        await Assert.That(result).IsEqualTo(TimeSingle.Zero);
    }

    [Test]
    public async Task ConvertFrom_WithInvalidString_ThrowsFormatExceptionNamingTheInput()
    {
        // Act
        var act = () => converter.ConvertFrom(null, CultureInfo.InvariantCulture, "invalid");

        // Assert
        var exception = await Assert.That(act).ThrowsExactly<FormatException>();
        await Assert.That(exception!.Message).Contains("Unable to parse 'invalid' as TimeSingle");
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
        var act = () => converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123.45f);

        // Assert
        await Assert.That(act).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertTo_WithStringType_ReturnsFormattedTime()
    {
        // Arrange
        var time = new TimeSingle(83.456f);

        // Act
        var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, time, typeof(string));

        // Assert
        await Assert.That(result).IsEqualTo("1:23.456");
    }

    [Test]
    public async Task ConvertTo_WithNonStringType_ThrowsNotSupportedException()
    {
        // Arrange
        var time = new TimeSingle(1f);

        // Act
        var act = () => converter.ConvertTo(null, CultureInfo.InvariantCulture, time, typeof(float));

        // Assert
        await Assert.That(act).ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task ConvertFrom_WithConvertToOutput_ReturnsOriginalTime()
    {
        // Arrange
        var original = new TimeSingle(123.456f);
        var text = (string)converter.ConvertTo(null, CultureInfo.InvariantCulture, original, typeof(string))!;

        // Act
        var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, text);

        // Assert
        await Assert.That(result).IsEqualTo(original);
    }
}
