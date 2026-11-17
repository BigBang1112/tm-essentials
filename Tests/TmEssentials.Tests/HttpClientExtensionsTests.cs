using RichardSzalay.MockHttp;
using System.Net;

namespace TmEssentials.Tests;

public class HttpClientExtensionsTests
{
    private const string Url = "https://example.com/";

    private readonly MockHttpMessageHandler mockHandler = new();
    private readonly HttpClient httpClient;

    public HttpClientExtensionsTests()
    {
        mockHandler.When(Url).Respond(HttpStatusCode.OK);
        httpClient = new HttpClient(mockHandler);
    }

    [After(Test)]
    public void Cleanup()
    {
        httpClient.Dispose();
    }

    [Test]
    public async Task HeadAsync_WithStringUri_SendsHeadRequest()
    {
        // Act
        var response = await httpClient.HeadAsync(Url);

        // Assert
        await Assert.That(response.RequestMessage!.Method).IsEqualTo(HttpMethod.Head);
    }

    [Test]
    public async Task HeadAsync_WithUri_SendsHeadRequest()
    {
        // Act
        var response = await httpClient.HeadAsync(new Uri(Url));

        // Assert
        await Assert.That(response.RequestMessage!.Method).IsEqualTo(HttpMethod.Head);
    }
}
