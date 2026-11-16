using System.Text.Json;
using TmEssentials.Converters;

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

    [Test]
    public async Task Serialize_Root_WritesNumber()
    {
        var options = CreateOptions();

        var json = JsonSerializer.Serialize(new TimeInt32(1234), options);

        await Assert.That(json).IsEqualTo("1234");
    }

    [Test]
    public async Task Deserialize_Root_ReadsNumber()
    {
        var options = CreateOptions();

        var value = JsonSerializer.Deserialize<TimeInt32>("1234", options);

        await Assert.That(value.TotalMilliseconds).IsEqualTo(1234);
    }

    [Test]
    public async Task Serialize_Property_WritesNumber()
    {
        var options = CreateOptions();

        var json = JsonSerializer.Serialize(new SampleContainer { Time = new TimeInt32(98765) }, options);

        await Assert.That(json).IsEqualTo("{\"Time\":98765}");
    }

    [Test]
    public async Task Deserialize_Property_ReadsNumber()
    {
        var options = CreateOptions();

        var obj = JsonSerializer.Deserialize<SampleContainer>("{\"Time\":98765}", options);

        await Assert.That(obj).IsNotNull();
        await Assert.That(obj!.Time.TotalMilliseconds).IsEqualTo(98765);
    }

    [Test]
    public async Task NegativeValue_SerializesCorrectly()
    {
        var options = CreateOptions();
        const int ms = -123456;

        var json = JsonSerializer.Serialize(new TimeInt32(ms), options);
        var back = JsonSerializer.Deserialize<TimeInt32>(json, options);

        await Assert.That(json).IsEqualTo(ms.ToString());
        await Assert.That(back.TotalMilliseconds).IsEqualTo(ms);
    }
}
#endif
