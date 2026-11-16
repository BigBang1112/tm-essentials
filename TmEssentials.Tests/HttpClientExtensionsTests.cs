using RichardSzalay.MockHttp;
using System.Net;

namespace TmEssentials.Tests;

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

    [After(Test)]
    public void Cleanup()
    {
        httpClient.Dispose();
    }

    [Test]
    public async Task HeadAsync_String_ReturnsMessageWithHeadOnly()
    {
        var response = await httpClient.HeadAsync("http://www.google.com");

        await Assert.That(response.RequestMessage!.Method).IsEqualTo(HttpMethod.Head);
    }

    [Test]
    public async Task HeadAsync_Uri_ReturnsMessageWithHeadOnly()
    {
        var response = await httpClient.HeadAsync(new Uri("http://www.google.com"));

        await Assert.That(response.RequestMessage!.Method).IsEqualTo(HttpMethod.Head);
    }
}
