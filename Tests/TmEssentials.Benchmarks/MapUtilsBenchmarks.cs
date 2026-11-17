using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class MapUtilsBenchmarks
{
    // A real map UID from ManiaPlanet, with its UUID and CRC32
    private const string MapUid = "az8D4K6OKOuyGtUjHV_ui39x4xi";
    private static readonly Guid Uuid = Guid.Parse("eabd691c-a87f-428c-9e56-608e21bc91dd");
    private const uint Crc32 = 0xCF9B930Du;

    [Benchmark]
    public string GenerateMapUid() => MapUtils.GenerateMapUid(Uuid, Crc32);

    [Benchmark]
    public bool TryDecodeMapUid() => MapUtils.TryDecodeMapUid(MapUid, out _, out _);
}
