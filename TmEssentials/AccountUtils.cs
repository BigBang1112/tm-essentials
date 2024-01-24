namespace TmEssentials;

/// <summary>
/// Provides utility methods for Trackmania accounts.
/// </summary>
public static class AccountUtils
{
    private const int GuidByteLength = 16;

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

        Span<char> chars = stackalloc char[32];
        _ = Convert.TryToBase64Chars(span, chars, out var charsWritten);

        for (int i = 0; i < charsWritten; i++)
        {
            if (chars[i] == '=')
            {
                return chars.Slice(0, i).ToString();
            }

            if (chars[i] == '+')
            {
                chars[i] = '-';
                continue;
            }

            if (chars[i] == '/')
            {
                chars[i] = '_';
                continue;
            }
        }

        return chars.Slice(0, charsWritten).ToString();

#endif
    }

    /// <summary>
    /// Converts a login to an account ID.
    /// </summary>
    /// <param name="login">The login to be converted.</param>
    /// <returns>An account ID of the login.</returns>
    /// <exception cref="ArgumentNullException">Login is null.</exception>
    /// <exception cref="FormatException">Login is invalid.</exception>
    public static Guid ToAccountId(string login)
    {
        if (login is null)
        {
            throw new ArgumentNullException(nameof(login));
        }

        var base64 = login.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

#if NETSTANDARD2_0 || NET462_OR_GREATER

        var b = Convert.FromBase64String(base64);

        return new Guid([
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[8], b[9],
            b[10], b[11], b[12], b[13], b[14], b[15]
        ]);
#else

        Span<byte> b = stackalloc byte[GuidByteLength];

        if (!Convert.TryFromBase64String(base64, b, out var _))
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
