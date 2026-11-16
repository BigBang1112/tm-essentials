namespace TmEssentials.Tests;

public class MapUtilsTests
{
    // Real map UIDs from ManiaPlanet and Trackmania (2020), decoded into their UUID and CRC32
    [Test]
    [Arguments("az8D4K6OKOuyGtUjHV_ui39x4xi", "eabd691c-a87f-428c-9e56-608e21bc91dd", 0xCF9B930Du)]
    [Arguments("xJtHiLrE2UZd6cDBySdiP4QtM24", "cf838652-ce60-4ac3-bfea-cfd99dbc19f2", 0x35C25CBEu)]
    [Arguments("A_3XSKEnElrOl0OdlvyTt1cjp3", "75744e08-87f4-402a-8e4d-0a2a529e35a6", 0x00BEB8ACu)]
    [Arguments("_mhf2U1Mp2RnBPKWSOWbhQdPNyf", "56e58a2a-622c-4e18-90b1-c6f7f1f3bc2b", 0xB0066AE4u)]
    public async Task GenerateMapUid_WithUuidAndCrc32_ReturnsGameUid(string expected, string uuid, uint crc32)
    {
        // Act
        var uid = MapUtils.GenerateMapUid(Guid.Parse(uuid), crc32);

        // Assert
        await Assert.That(uid).IsEqualTo(expected);
    }

    [Test]
    [Arguments("az8D4K6OKOuyGtUjHV_ui39x4xi", "eabd691c-a87f-428c-9e56-608e21bc91dd", 0xCF9B930Du)]
    [Arguments("A_3XSKEnElrOl0OdlvyTt1cjp3", "75744e08-87f4-402a-8e4d-0a2a529e35a6", 0x00BEB8ACu)]
    public async Task TryDecodeMapUid_WithGameUid_ReturnsUuidAndCrc32(string mapUid, string expectedUuid, uint expectedCrc32)
    {
        // Act
        var success = MapUtils.TryDecodeMapUid(mapUid, out var uuid, out var crc32);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(uuid).IsEqualTo(Guid.Parse(expectedUuid));
        await Assert.That(crc32).IsEqualTo(expectedCrc32);
    }

    [Test]
    [Arguments(null)]
    [Arguments("az8D4K6OKOuyGtUjHV_ui39x4x-")]
    [Arguments("az8D4K6OKOuyGtUjHV_ui39x4xia")]
    [Arguments("ZZZZZZZZZZZZZZZZZZZZZZZZZZZ")]
    public async Task TryDecodeMapUid_WithInvalidUid_ReturnsFalse(string? mapUid)
    {
        // Act
        var success = MapUtils.TryDecodeMapUid(mapUid, out _, out _);

        // Assert
        await Assert.That(success).IsFalse();
    }

    [Test]
    public async Task GenerateMapUid_WithEmptyUuidAndZeroCrc32_ReturnsEmptyString()
    {
        // Act
        var uid = MapUtils.GenerateMapUid(Guid.Empty, 0);

        // Assert
        await Assert.That(uid).IsEmpty();
    }

    [Test]
    public async Task GenerateMapUid_WithSeed_ReturnsDeterministicUid()
    {
        // Act
        var first = MapUtils.GenerateMapUid(1112);
        var second = MapUtils.GenerateMapUid(1112);

        // Assert
        await Assert.That(first).IsEqualTo(second);
    }

    [Test]
    [Arguments(1)]
    [Arguments(1112)]
    [Arguments(424242)]
    public async Task GenerateMapUid_WithSeed_ContainsVersion4Uuid(int seed)
    {
        // Act
        var uid = MapUtils.GenerateMapUid(seed);
        MapUtils.TryDecodeMapUid(uid, out var uuid, out _);

        // Assert
        await Assert.That(uuid.ToString()[14]).IsEqualTo('4');
        await Assert.That(uuid.ToString()[19] is '8' or '9' or 'a' or 'b').IsTrue();
    }

    [Test]
    public async Task GenerateMapUid_WithNullRandom_ThrowsArgumentNullException()
    {
        // Act
        var act = () => MapUtils.GenerateMapUid(random: null!);

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task GenerateMapUid_WithoutArguments_ReturnsDecodableUid()
    {
        // Act
        var uid = MapUtils.GenerateMapUid();

        // Assert
        await Assert.That(uid.Length).IsLessThanOrEqualTo(MapUtils.MaxMapUidLength);
        await Assert.That(MapUtils.TryDecodeMapUid(uid, out _, out _)).IsTrue();
    }
}
