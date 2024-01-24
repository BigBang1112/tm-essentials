using RichardSzalay.MockHttp;
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
    private readonly MockHttpMessageHandler mockHandler = new();
    private readonly HttpClient httpClient;

    public HttpClientExtensionsTests()
    {
        mockHandler.When("http://www.google.com")
            .Respond(HttpStatusCode.OK);
        httpClient = new HttpClient(mockHandler);
    }

    [Fact]
    public async Task HeadAsync_String_ReturnsMessageWithHeadOnly()
    {
        var response = await httpClient.HeadAsync("http://www.google.com");

        Assert.Equal(expected: HttpMethod.Head, actual: response.RequestMessage.Method);
    }

    [Fact]
    public async Task HeadAsync_Uri_ReturnsMessageWithHeadOnly()
    {
        var response = await httpClient.HeadAsync(new Uri("http://www.google.com"));

        Assert.Equal(expected: HttpMethod.Head, actual: response.RequestMessage.Method);
    }
}

#endif
