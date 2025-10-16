using System.Text.Json;
using TmEssentials.Converters;
using Xunit;

#if NET5_0_OR_GREATER
namespace TmEssentials.Tests;

public sealed class JsonTimeInt32ConverterTests
{
    private static JsonSerializerOptions CreateOptions()
    {
        var o = new JsonSerializerOptions();
        o.Converters.Add(new JsonTimeInt32Converter());
        return o;
    }

    private sealed class SampleContainer
    {
        public TimeInt32 Time { get; set; }
    }

    [Fact]
    public void Serialize_Root_WritesNumber()
    {
        var options = CreateOptions();

        var json = JsonSerializer.Serialize(new TimeInt32(1234), options);

        Assert.Equal("1234", json);
    }

    [Fact]
    public void Deserialize_Root_ReadsNumber()
    {
        var options = CreateOptions();

        var value = JsonSerializer.Deserialize<TimeInt32>("1234", options);

        Assert.Equal(1234, value.TotalMilliseconds);
    }

    [Fact]
    public void Serialize_Property_WritesNumber()
    {
        var options = CreateOptions();

        var json = JsonSerializer.Serialize(new SampleContainer { Time = new TimeInt32(98765) }, options);

        Assert.Equal("{\"Time\":98765}", json);
    }

    [Fact]
    public void Deserialize_Property_ReadsNumber()
    {
        var options = CreateOptions();

        var obj = JsonSerializer.Deserialize<SampleContainer>("{\"Time\":98765}", options);

        Assert.NotNull(obj);
        Assert.Equal(98765, obj!.Time.TotalMilliseconds);
    }

    [Fact]
    public void NegativeValue_SerializesCorrectly()
    {
        var options = CreateOptions();
        const int ms = -123456;

        var json = JsonSerializer.Serialize(new TimeInt32(ms), options);
        var back = JsonSerializer.Deserialize<TimeInt32>(json, options);

        Assert.Equal(ms.ToString(), json);
        Assert.Equal(ms, back.TotalMilliseconds);
    }
}
#endif
