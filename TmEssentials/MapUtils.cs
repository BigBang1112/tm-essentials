namespace TmEssentials;

/// <summary>
/// Provides utility methods for Trackmania, Shootmania, and Questmania maps.
/// </summary>
public static class MapUtils
{
#if !NET5_0_OR_GREATER
    private static readonly Random random = new();
#endif

    /// <summary>
    /// Generates a random 27-character map UID. The implementation is approximate and NOT the same as in the game.
    /// </summary>
    /// <param name="random">Instance of <see cref="Random"/> to use for generating the UID.</param>
    /// <returns>A random map UID.</returns>
    public static string GenerateMapUid(Random random)
    {
        // Official string used for modern map UIDs
        const string chars = "_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // MapUid is most often 27 characters long
#if NETSTANDARD2_0 || NET462_OR_GREATER
        var mapUid = new char[27];
#else
        Span<char> mapUid = stackalloc char[27];
#endif

        for (var i = 0; i < mapUid.Length; i++)
        {
            mapUid[i] = chars[random.Next(chars.Length)];
        }

        return new string(mapUid);
    }

    /// <summary>
    /// Generates a random 27-character map UID. The implementation is approximate and NOT the same as in the game.
    /// </summary>
    /// <param name="seed">Seed for the UID generator.</param>
    /// <returns>A consistent map UID, based on the seed.</returns>
    public static string GenerateMapUid(int seed)
    {
        return GenerateMapUid(new Random(seed));
    }

    /// <summary>
    /// Generates a random 27-character map UID. The implementation is approximate and NOT the same as in the game.
    /// </summary>
    /// <returns>A random map UID.</returns>
    public static string GenerateMapUid()
    {
#if NET5_0_OR_GREATER
        return GenerateMapUid(Random.Shared);
#else
        return GenerateMapUid(random);
#endif
    }
}
