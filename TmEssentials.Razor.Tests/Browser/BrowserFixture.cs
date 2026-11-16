using Microsoft.Playwright;
using TUnit.Core.Interfaces;

namespace TmEssentials.Razor.Tests.Browser;

/// <summary>
/// One browser shared by all tests. Uses the installed Chrome, so there is no separate browser download.
/// </summary>
public sealed class BrowserFixture : IAsyncInitializer, IAsyncDisposable
{
    private IPlaywright? playwright;

    public IBrowser Browser { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        playwright = await Playwright.CreateAsync();
        Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome" });
    }

    public async ValueTask DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.DisposeAsync();
        }

        playwright?.Dispose();
    }
}
