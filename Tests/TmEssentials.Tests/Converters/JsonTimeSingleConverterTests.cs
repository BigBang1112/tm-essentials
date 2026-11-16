using System.Text.Json;
using TmEssentials.Converters;

namespace TmEssentials.Tests.Converters;

public sealed class JsonTimeSingleConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonTimeSingleConverter() }
    };

    private sealed record SampleContainer(TimeSingle Time);

    [Test]
    [Arguments(1_234.5f, "1234.5")]
    [Arguments(-123_456.75f, "-123456.75")]
    public async Task Serialize_Time_WritesTotalSecondsAsNumber(float totalSeconds, string expectedJson)
    {
        // Arrange
        var time = new TimeSingle(totalSeconds);

        // Act
        var json = JsonSerializer.Serialize(time, Options);

        // Assert
        await Assert.That(json).IsEqualTo(expectedJson);
    }

    [Test]
    [Arguments("1234.5", 1_234.5f)]
    [Arguments("-123456.75", -123_456.75f)]
    public async Task Deserialize_Number_ReadsTotalSeconds(string json, float expectedTotalSeconds)
    {
        // Act
        var time = JsonSerializer.Deserialize<TimeSingle>(json, Options);

        // Assert
        await Assert.That(time).IsEqualTo(new TimeSingle(expectedTotalSeconds));
    }

    [Test]
    public async Task Serialize_TimeProperty_WritesNumber()
    {
        // Arrange
        var container = new SampleContainer(new TimeSingle(98_765.125f));

        // Act
        var json = JsonSerializer.Serialize(container, Options);

        // Assert
        await Assert.That(json).IsEqualTo("{\"Time\":98765.125}");
    }

    [Test]
    public async Task Deserialize_TimeProperty_ReadsNumber()
    {
        // Act
        var container = JsonSerializer.Deserialize<SampleContainer>("{\"Time\":98765.125}", Options);

        // Assert
        await Assert.That(container).IsEqualTo(new SampleContainer(new TimeSingle(98_765.125f)));
    }
}
