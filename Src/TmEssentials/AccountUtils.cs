namespace TmEssentials;

/// <summary>
/// Provides utility methods for Trackmania accounts.
/// </summary>
public static class AccountUtils
{
#if NETSTANDARD2_1 || NET8_0_OR_GREATER
    private const int GuidByteLength = 16;
    private const int MaxStackBase64Length = 64;
#endif

    /// <summary>
    /// Converts an account ID to a login representation.
    /// </summary>
    /// <param name="accountId">The account ID to be converted.</param>
    /// <returns>A login representation of the account ID.</returns>
    public static string ToLogin(Guid accountId)
    {
#if NETSTANDARD2_0 || NET462_OR_GREATER

        var b = accountId.ToByteArray();

        var str = Convert.ToBase64String([
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[8], b[9],
            b[10], b[11], b[12], b[13], b[14], b[15]]);

        return str.Replace('+', '-').Replace('/', '_').Replace("=", "");

#else

#if NET8_0_OR_GREATER

        Span<byte> span = stackalloc byte[GuidByteLength];

        // False whenever the span is lower than 16 bytes.
        _ = accountId.TryWriteBytes(span, bigEndian: true, out int len);

#else
        
        Span<byte> b = stackalloc byte[GuidByteLength];

        accountId.TryWriteBytes(b);

        Span<byte> span = stackalloc byte[]
        {
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[8], b[9],
            b[10], b[11], b[12], b[13], b[14], b[15]
        };

#endif

        Span<char> chars = stackalloc char[24];
        _ = Convert.TryToBase64Chars(span, chars, out _);

        // 16 bytes are always 22 base64 characters followed by "==" padding, which is not part of the login
        var login = chars.Slice(0, 22);

        for (int i = 0; i < login.Length; i++)
        {
            switch (login[i])
            {
                case '+':
                    login[i] = '-';
                    break;
                case '/':
                    login[i] = '_';
                    break;
            }
        }

        return login.ToString();

#endif
    }

    /// <summary>
    /// Converts a login to an account ID.
    /// </summary>
    /// <param name="login">The login to be converted.</param>
    /// <returns>An account ID of the login.</returns>
    /// <exception cref="ArgumentNullException">Login is null.</exception>
    /// <exception cref="FormatException">Login is not base64url, or it does not decode to exactly 16 bytes.</exception>
    public static Guid ToAccountId(string login)
    {
        if (login is null)
        {
            throw new ArgumentNullException(nameof(login));
        }

        // Base64url without the padding, so the padding is added back
        var paddingLength = (login.Length % 4) switch
        {
            2 => 2,
            3 => 1,
            _ => 0
        };

#if NETSTANDARD2_0 || NET462_OR_GREATER

        var base64 = login.Replace('-', '+').Replace('_', '/') + new string('=', paddingLength);

        var b = Convert.FromBase64String(base64);

        if (b.Length != 16)
        {
            throw new FormatException("Invalid login.");
        }

        return new Guid([
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[8], b[9],
            b[10], b[11], b[12], b[13], b[14], b[15]
        ]);
#else

        // A login has 22 characters. Longer input can still decode thanks to the whitespace that base64 skips, so it goes to the heap.
        var base64Length = login.Length + paddingLength;
        Span<char> base64 = base64Length <= MaxStackBase64Length ? stackalloc char[MaxStackBase64Length] : new char[base64Length];
        base64 = base64.Slice(0, base64Length);

        for (var i = 0; i < login.Length; i++)
        {
            var c = login[i];

            base64[i] = c switch
            {
                '-' => '+',
                '_' => '/',
                _ => c
            };
        }

        base64.Slice(login.Length).Fill('=');

        Span<byte> b = stackalloc byte[GuidByteLength];

        // Fails when the login is longer than 16 bytes, and the written length catches the shorter ones
        if (!Convert.TryFromBase64Chars(base64, b, out var bytesWritten) || bytesWritten != GuidByteLength)
        {
            throw new FormatException("Invalid login.");
        }

#if NET8_0_OR_GREATER

        return new Guid(b, bigEndian: true);

#else

        Span<byte> span = stackalloc byte[]
        {
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[8], b[9],
            b[10], b[11], b[12], b[13], b[14], b[15]
        };

        return new Guid(span);
#endif

#endif
    }
}
