using Microsoft.Playwright;

namespace TmEssentials.Razor.Tests.Browser;

/// <summary>
/// Opens pages in a real browser. Requests to a made-up origin are answered with the page HTML
/// and the static web assets of the library, so nothing has to be hosted.
/// </summary>
public abstract class BrowserTestBase
{
    protected const string StylesheetLink = """<link rel="stylesheet" href="/tm-essentials.css" />""";

    private const string Origin = "http://tm-essentials.test";

    private IBrowserContext context = default!;
    private string html = "";

    [ClassDataSource<BrowserFixture>(Shared = SharedType.PerTestSession)]
    public required BrowserFixture Fixture { get; init; }

    protected IPage Page { get; private set; } = default!;

    [Before(Test)]
    public async Task OpenPageAsync()
    {
        context = await Fixture.Browser.NewContextAsync();
        Page = await context.NewPageAsync();

        await Page.RouteAsync($"{Origin}/**", ServeAsync);
    }

    [After(Test)]
    public async Task ClosePageAsync()
    {
        await context.CloseAsync();
    }

    protected async Task GoToAsync(string body, string head = StylesheetLink)
    {
        html = $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8" />
                {head}
            </head>
            <body>{body}</body>
            </html>
            """;

        await Page.GotoAsync($"{Origin}/");
    }

    private async Task ServeAsync(IRoute route)
    {
        var path = new Uri(route.Request.Url).AbsolutePath;

        if (path == "/")
        {
            await route.FulfillAsync(new RouteFulfillOptions { ContentType = "text/html", Body = html });
            return;
        }

        var file = Path.Combine(AppContext.BaseDirectory, "wwwroot", path.TrimStart('/'));

        if (!File.Exists(file))
        {
            await route.FulfillAsync(new RouteFulfillOptions { Status = 404 });
            return;
        }

        await route.FulfillAsync(new RouteFulfillOptions
        {
            ContentType = Path.GetExtension(file) switch
            {
                ".css" => "text/css",
                ".woff2" => "font/woff2",
                _ => "application/octet-stream"
            },
            BodyBytes = await File.ReadAllBytesAsync(file)
        });
    }
}
