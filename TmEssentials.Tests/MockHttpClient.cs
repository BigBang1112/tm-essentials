using System;

#if NET6_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER
using System.Net.Http;
#endif

using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace TmEssentials.Tests;

#if NET6_0_OR_GREATER || NETCOREAPP3_1_OR_GREATER

public class MockHttpClient : HttpClient
{
    public MockHttpClient(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> handler) : base(CreateMockMessageHandler(handler).Object)
    {

    }

    private static Mock<HttpMessageHandler> CreateMockMessageHandler(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> handler)
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(handler);

        return mockHttpMessageHandler;
    }
}

#endif
