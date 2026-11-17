namespace TmEssentials.Razor.Tests.Browser;

public class IconFontTests : BrowserTestBase
{
    private const string FontFamily = "TmEssentials Font Awesome";

    /// <summary>
    /// Draws the text with the icon font first in the font list and without it, and tells whether the drawings differ.
    /// When the icon font has no glyph for the text, the browser falls back to the same font and both drawings are equal.
    /// </summary>
    private Task<bool> IsDrawnWithIconFontAsync(string text) => Page.EvaluateAsync<bool>($$"""
        async text => {
            await document.fonts.load('32px "{{FontFamily}}"', text);

            const draw = font => {
                const canvas = document.createElement("canvas");
                canvas.width = 64;
                canvas.height = 64;
                const context = canvas.getContext("2d");
                context.font = font;
                context.fillText(text, 8, 48);
                return canvas.toDataURL();
            };

            return draw('32px "{{FontFamily}}", monospace') !== draw("32px monospace");
        }
        """, text);

    [Test]
    public async Task Load_WithIconOnPage_LoadsBundledFont()
    {
        // Arrange
        await GoToAsync("""<span class="tm-icon">&#xF007;</span>""");

        // Act
        var status = await Page.EvaluateAsync<string?>($$"""
            async () => {
                await document.fonts.ready;
                return [...document.fonts].find(font => font.family.replaceAll('"', "") === "{{FontFamily}}")?.status;
            }
            """);

        // Assert
        await Assert.That(status).IsEqualTo("loaded");
    }

    [Test]
    [Arguments("")] // first icon of Font Awesome 4.7
    [Arguments("")] // user
    [Arguments("")] // trophy
    [Arguments("")] // checkered flag
    [Arguments("")] // microchip, the last icon listed by the Trackmania wiki
    [Arguments("")] // last icon of Font Awesome 4.7
    public async Task Draw_WithFontAwesomeIcon_UsesBundledFont(string icon)
    {
        // Arrange
        await GoToAsync("");

        // Act
        var isDrawnWithIconFont = await IsDrawnWithIconFontAsync(icon);

        // Assert
        await Assert.That(isDrawnWithIconFont).IsTrue();
    }

    [Test]
    [Arguments("A")]
    [Arguments("1")]
    [Arguments("")] // PlayStation, a Trackmania platform icon that is not in Font Awesome
    public async Task Draw_WithCharacterOutsideIconRange_DoesNotUseBundledFont(string text)
    {
        // Arrange
        await GoToAsync("");

        // Act
        var isDrawnWithIconFont = await IsDrawnWithIconFontAsync(text);

        // Assert
        await Assert.That(isDrawnWithIconFont).IsFalse();
    }

    [Test]
    public async Task Render_WithFormattedIcon_StylesIconWithFontAndColor()
    {
        // Arrange
        var html = TextFormatter.FormatHtml("$f00 Winner", allowLinks: false, allowManialinks: false, FormattedText.DefaultIconCssClass);
        await GoToAsync(html);

        // Act
        var style = await Page.Locator(".tm-icon").EvaluateAsync<string[]>("""
            icon => {
                const style = getComputedStyle(icon);
                return [style.fontFamily, style.color];
            }
            """);

        // Assert
        await Assert.That(style).IsEquivalentTo([$"\"{FontFamily}\"", "rgb(255, 0, 0)"]);
    }
}
