using System.Buffers.Binary;

namespace TmEssentials;

/// <summary>
/// Provides utility methods for Trackmania, Shootmania, and Questmania maps.
/// </summary>
public static class MapUtils
{
    // Digits of the base-63 number that a map UID is (CFastString::EncodeData in the game)
    private const string MapUidChars = "_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int MapUidBase = 63;

    // 16 bytes of the UUID + 4 bytes of the CRC32
    private const int MapUidDataLength = 20;

    /// <summary>
    /// The maximum length of a map UID made by <see cref="GenerateMapUid(Guid, uint)"/>. 63^27 is the first power of 63 above 2^160.
    /// </summary>
    public const int MaxMapUidLength = 27;

    /// <summary>
    /// Generates a map UID from a UUID and a CRC32, the same way as the game does in <c>CGameCtnChallenge::GenerateUid</c>.
    /// </summary>
    /// <remarks>
    /// The game takes the 16 bytes of a new UUID followed by the 4 bytes of the map's CRC32 (little-endian), reads them as one
    /// little-endian number, and writes it in base 63 from the least significant digit, using <c>_0-9a-zA-Z</c> as the digits.
    /// The UID is therefore most often 27 characters long, but it is shorter when the number is small enough (about 1 in 24).
    /// </remarks>
    /// <param name="uuid">The UUID. The game uses a new random (version 4) UUID.</param>
    /// <param name="crc32">The CRC32. The game uses the CRC32 of the map's serialized data, computed with an empty map name.</param>
    /// <returns>The map UID. It is empty only when both <paramref name="uuid"/> and <paramref name="crc32"/> are zero.</returns>
    public static string GenerateMapUid(Guid uuid, uint crc32)
    {
        Span<byte> data = stackalloc byte[MapUidDataLength];
        WriteGuid(uuid, data);
        BinaryPrimitives.WriteUInt32LittleEndian(data.Slice(16), crc32);

#if NETSTANDARD2_0 || NET462_OR_GREATER
        var mapUid = new char[MaxMapUidLength];
#else
        Span<char> mapUid = stackalloc char[MaxMapUidLength];
#endif

        var length = 0;

        while (!IsZero(data))
        {
            mapUid[length++] = MapUidChars[DivideBy(data, MapUidBase)];
        }

#if NETSTANDARD2_0 || NET462_OR_GREATER
        return new string(mapUid, 0, length);
#else
        return new string(mapUid.Slice(0, length));
#endif
    }

    /// <summary>
    /// Generates a random map UID, the same way as the game does, with random bytes in place of the UUID and the CRC32.
    /// </summary>
    /// <remarks>
    /// The UUID bytes get the version 4 and variant bits, like the UUIDs that the game uses.
    /// </remarks>
    /// <param name="random">Instance of <see cref="Random"/> to use for generating the UID.</param>
    /// <returns>A random map UID, most often 27 characters long.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="random"/> is null.</exception>
    public static string GenerateMapUid(Random random)
    {
        if (random is null)
        {
            throw new ArgumentNullException(nameof(random));
        }

#if NETSTANDARD2_0 || NET462_OR_GREATER
        var buffer = new byte[MapUidDataLength];
        random.NextBytes(buffer);
        Span<byte> data = buffer;
#else
        Span<byte> data = stackalloc byte[MapUidDataLength];
        random.NextBytes(data);
#endif

        // Version 4 in the high nibble of byte 7, and the RFC 4122 variant in the 2 high bits of byte 8
        data[7] = (byte)((data[7] & 0x0F) | 0x40);
        data[8] = (byte)((data[8] & 0x3F) | 0x80);

        return GenerateMapUid(ReadGuid(data), BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(16)));
    }

    /// <summary>
    /// Generates a random map UID from a seed, the same way as the game does, with random bytes in place of the UUID and the CRC32.
    /// </summary>
    /// <param name="seed">Seed for the UID generator.</param>
    /// <returns>A consistent map UID, based on the seed.</returns>
    public static string GenerateMapUid(int seed)
    {
        return GenerateMapUid(new Random(seed));
    }

    /// <summary>
    /// Generates a random map UID, the same way as the game does, from a new UUID and a random number in place of the CRC32.
    /// </summary>
    /// <returns>A random map UID, most often 27 characters long.</returns>
    public static string GenerateMapUid()
    {
        // The first 4 bytes of a version 4 UUID are all random, and unlike a shared Random, it is thread-safe on every target
        Span<byte> crc32 = stackalloc byte[16];
        WriteGuid(Guid.NewGuid(), crc32);

        return GenerateMapUid(Guid.NewGuid(), BinaryPrimitives.ReadUInt32LittleEndian(crc32));
    }

    /// <summary>
    /// Decodes a map UID made by the game (or by <see cref="GenerateMapUid(Guid, uint)"/>) back into its UUID and CRC32.
    /// </summary>
    /// <remarks>
    /// ManiaPlanet, Trackmania (2020) and TrackMania Forever all make map UIDs this way. A UID that was made differently
    /// can still decode, but its UUID is then very unlikely to be a version 4 UUID.
    /// </remarks>
    /// <param name="mapUid">The map UID.</param>
    /// <param name="uuid">The UUID, if decoded.</param>
    /// <param name="crc32">The CRC32, if decoded.</param>
    /// <returns>True if the map UID consists only of base-63 digits and fits into 20 bytes, otherwise false.</returns>
    public static bool TryDecodeMapUid(string? mapUid, out Guid uuid, out uint crc32)
    {
        uuid = Guid.Empty;
        crc32 = 0;

        if (mapUid is null || mapUid.Length > MaxMapUidLength)
        {
            return false;
        }

        Span<byte> data = stackalloc byte[MapUidDataLength];

        // The first character is the least significant digit
        for (var i = mapUid.Length - 1; i >= 0; i--)
        {
            var digit = MapUidChars.IndexOf(mapUid[i]);

            if (digit < 0 || !MultiplyAdd(data, MapUidBase, (byte)digit))
            {
                return false;
            }
        }

        uuid = ReadGuid(data);
        crc32 = BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(16));

        return true;
    }

    private static bool IsZero(ReadOnlySpan<byte> number)
    {
        foreach (var b in number)
        {
            if (b != 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Divides a little-endian number in place and returns the remainder (DivBigNat in the game).
    /// </summary>
    private static int DivideBy(Span<byte> number, int divisor)
    {
        var remainder = 0;

        for (var i = number.Length - 1; i >= 0; i--)
        {
            var value = (remainder << 8) | number[i];
            number[i] = (byte)(value / divisor);
            remainder = value % divisor;
        }

        return remainder;
    }

    /// <summary>
    /// Multiplies a little-endian number in place and adds a value to it. Returns false when the result overflows.
    /// </summary>
    private static bool MultiplyAdd(Span<byte> number, int multiplier, byte addend)
    {
        var carry = (int)addend;

        for (var i = 0; i < number.Length; i++)
        {
            var value = number[i] * multiplier + carry;
            number[i] = (byte)value;
            carry = value >> 8;
        }

        return carry == 0;
    }

    private static void WriteGuid(Guid guid, Span<byte> destination)
    {
#if NETSTANDARD2_0 || NET462_OR_GREATER
        guid.ToByteArray().CopyTo(destination);
#else
        guid.TryWriteBytes(destination);
#endif
    }

    private static Guid ReadGuid(ReadOnlySpan<byte> source)
    {
#if NETSTANDARD2_0 || NET462_OR_GREATER
        return new Guid(source.Slice(0, 16).ToArray());
#else
        return new Guid(source.Slice(0, 16));
#endif
    }
}
