namespace TmEssentials;

public static class AccountUtils
{
    public static string ToLogin(Guid guid)
    {
#if NETSTANDARD2_0 || NET462_OR_GREATER

        var b = guid.ToByteArray();

        var str = Convert.ToBase64String([
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[8], b[9],
            b[10], b[11], b[12], b[13], b[14], b[15]]);

        return str.Replace('+', '-').Replace('/', '_').Replace("=", "");

#else

#if NET8_0_OR_GREATER

        Span<byte> span = stackalloc byte[16];

        guid.TryWriteBytes(span, true, out int len);

#else
        
        Span<byte> b = stackalloc byte[16];

        guid.TryWriteBytes(b);

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
}
