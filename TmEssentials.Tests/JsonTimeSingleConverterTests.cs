using System.Text.Json;
using TmEssentials.Converters;
using Xunit;

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

    [Fact]
    public void Serialize_Root_WritesNumber()
    {
        var options = CreateOptions();
        var json = JsonSerializer.Serialize(new TimeSingle(1234.5f), options);

        Assert.Equal("1234.5", json);
    }

    [Fact]
    public void Deserialize_Root_ReadsNumber()
    {
        var options = CreateOptions();
        var value = JsonSerializer.Deserialize<TimeSingle>("1234.5", options);

        Assert.Equal(1234.5f, value.TotalSeconds);
    }

    [Fact]
    public void Serialize_Property_WritesNumber()
    {
        var options = CreateOptions();
        var json = JsonSerializer.Serialize(new SampleContainer { Time = new TimeSingle(98765.125f) }, options);

        Assert.Equal("{\"Time\":98765.125}", json);
    }

    [Fact]
    public void Deserialize_Property_ReadsNumber()
    {
        var options = CreateOptions();
        var obj = JsonSerializer.Deserialize<SampleContainer>("{\"Time\":98765.125}", options);

        Assert.NotNull(obj);
        Assert.Equal(98765.125f, obj!.Time.TotalSeconds);
    }

    [Fact]
    public void NegativeValue_SerializesCorrectly()
    {
        var options = CreateOptions();
        const float secs = -123456.75f;
        var json = JsonSerializer.Serialize(new TimeSingle(secs), options);
        var back = JsonSerializer.Deserialize<TimeSingle>(json, options);

        Assert.Equal(secs, back.TotalSeconds);
    }
}
