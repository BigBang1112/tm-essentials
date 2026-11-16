using System.Text.Json;
using TmEssentials.Converters;

namespace TmEssentials.Tests;

public sealed class JsonTimeSingleConverterTests
{
    private static JsonSerializerOptions CreateOptions()
    {
        var o = new JsonSerializerOptions();
        o.Converters.Add(new JsonTimeSingleConverter());
        return o;
    }

    private sealed class SampleContainer
    {
        public TimeSingle Time { get; set; }
    }

    [Test]
    public async Task Serialize_Root_WritesNumber()
    {
        var options = CreateOptions();
        var json = JsonSerializer.Serialize(new TimeSingle(1234.5f), options);

        await Assert.That(json).IsEqualTo("1234.5");
    }

    [Test]
    public async Task Deserialize_Root_ReadsNumber()
    {
        var options = CreateOptions();
        var value = JsonSerializer.Deserialize<TimeSingle>("1234.5", options);

        await Assert.That(value.TotalSeconds).IsEqualTo(1234.5f);
    }

    [Test]
    public async Task Serialize_Property_WritesNumber()
    {
        var options = CreateOptions();
        var json = JsonSerializer.Serialize(new SampleContainer { Time = new TimeSingle(98765.125f) }, options);

        await Assert.That(json).IsEqualTo("{\"Time\":98765.125}");
    }

    [Test]
    public async Task Deserialize_Property_ReadsNumber()
    {
        var options = CreateOptions();
        var obj = JsonSerializer.Deserialize<SampleContainer>("{\"Time\":98765.125}", options);

        await Assert.That(obj).IsNotNull();
        await Assert.That(obj!.Time.TotalSeconds).IsEqualTo(98765.125f);
    }

    [Test]
    public async Task NegativeValue_SerializesCorrectly()
    {
        var options = CreateOptions();
        const float secs = -123456.75f;
        var json = JsonSerializer.Serialize(new TimeSingle(secs), options);
        var back = JsonSerializer.Deserialize<TimeSingle>(json, options);

        await Assert.That(back.TotalSeconds).IsEqualTo(secs);
    }
}
