using System;
using System.Net;

#if NET6_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER
using System.Net.Http;
using System.Threading.Tasks;
#endif

using Xunit;

namespace TmEssentials.Tests;

#if NET6_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER

public class HttpClientExtensionsTests
{
    [Fact]
    public async Task HeadAsync_ReturnsMessageWithHeadOnly()
    {
        var date = DateTimeOffset.Now;
        
        var httpClient = new MockHttpClient((req, token) =>
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = req
            };
        });
        
        var response = await httpClient.HeadAsync("http://www.google.com");

        Assert.Equal(expected: HttpMethod.Head, actual: response.RequestMessage.Method);
    }
}

#endif
