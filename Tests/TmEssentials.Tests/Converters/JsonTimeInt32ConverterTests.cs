using System.Text.Json;
using TmEssentials.Converters;

namespace TmEssentials.Tests.Converters;

public sealed class JsonTimeInt32ConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonTimeInt32Converter() }
    };

    private sealed record SampleContainer(TimeInt32 Time);

    [Test]
    [Arguments(1_234, "1234")]
    [Arguments(-123_456, "-123456")]
    public async Task Serialize_Time_WritesTotalMillisecondsAsNumber(int totalMilliseconds, string expectedJson)
    {
        // Arrange
        var time = new TimeInt32(totalMilliseconds);

        // Act
        var json = JsonSerializer.Serialize(time, Options);

        // Assert
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    [Arguments("1234", 1_234)]
    [Arguments("-123456", -123_456)]
    public async Task Deserialize_Number_ReadsTotalMilliseconds(string json, int expectedTotalMilliseconds)
    {
        // Act
        var time = JsonSerializer.Deserialize<TimeInt32>(json, Options);

        // Assert
        await Assert.That(time).IsEqualTo(new TimeInt32(expectedTotalMilliseconds));
    }

    [Test]
    public async Task Serialize_TimeProperty_WritesNumber()
    {
        // Arrange
        var container = new SampleContainer(new TimeInt32(98_765));

        // Act
        var json = JsonSerializer.Serialize(container, Options);

        // Assert
        await Assert.That(json).IsEqualTo("{\"Time\":98765}");
    }

    [Test]
    public async Task Deserialize_TimeProperty_ReadsNumber()
    {
        // Act
        var container = JsonSerializer.Deserialize<SampleContainer>("{\"Time\":98765}", Options);

        // Assert
        await Assert.That(container).IsEqualTo(new SampleContainer(new TimeInt32(98_765)));
    }
}
