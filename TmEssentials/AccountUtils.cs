namespace TmEssentials;

public static class AccountUtils
{
    public static string ToLogin(Guid guid)
    {
#if NET8_0_OR_GREATER

        Span<byte> span = stackalloc byte[16];

        guid.TryWriteBytes(span, true, out int len);

        var str = Convert.ToBase64String(span);

#elif NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER   
        
        Span<byte> b = stackalloc byte[16];

        guid.TryWriteBytes(b);

        var str = Convert.ToBase64String([
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[9], b[8],
            b[10], b[11], b[12], b[13], b[14], b[15]]);

#else

        var b = guid.ToByteArray();

        var str = Convert.ToBase64String([
            b[3], b[2], b[1], b[0],
            b[5], b[4],
            b[7], b[6],
            b[9], b[8],
            b[10], b[11], b[12], b[13], b[14], b[15]]);

#endif

        return str.Replace('+', '-').Replace('/', '_').Replace("=", "");
    }
}
