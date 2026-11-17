namespace TmEssentials.Razor.Tests;

public class FormattedTextTests : BunitContext
{
    private const string UserIcon = "";
    private const string TrophyIcon = "";
    private const string PlayStationIcon = "";

    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task Render_WithoutText_RendersNothing(string? text)
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, text));

        // Assert
        await Assert.That(cut.Markup).IsEmpty();
    }

    [Test]
    public void Render_WithColorCode_RendersColoredSpan()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, "$f00Red"));

        // Assert
        cut.MarkupMatches("""<span style="color:#f00">Red</span>""");
    }

    [Test]
    public async Task Render_WithIcon_WrapsIconInDefaultIconClass()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, $"{UserIcon} Player"));

        // Assert
        await Assert.That(cut.Find($"span.{FormattedText.DefaultIconCssClass}").TextContent).IsEqualTo(UserIcon);
    }

    [Test]
    public async Task Render_WithAdjacentIcons_WrapsThemInOneElement()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, $"{UserIcon}{TrophyIcon} Winner"));

        // Assert
        await Assert.That(cut.FindAll("span.tm-icon").Single().TextContent).IsEqualTo(UserIcon + TrophyIcon);
    }

    [Test]
    public void Render_WithColoredIcon_KeepsIconInsideColoredSpan()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, $"$f00{TrophyIcon}"));

        // Assert
        cut.MarkupMatches($"""<span style="color:#f00"><span class="tm-icon">{TrophyIcon}</span></span>""");
    }

    [Test]
    public async Task Render_WithCustomIconClass_UsesIt()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters
            .Add(x => x.Text, UserIcon)
            .Add(x => x.IconCssClass, "fa"));

        // Assert
        await Assert.That(cut.Find("span.fa").TextContent).IsEqualTo(UserIcon);
    }

    [Test]
    public async Task Render_WithoutIconClass_DoesNotWrapIcon()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters
            .Add(x => x.Text, $"{UserIcon} Player")
            .Add(x => x.IconCssClass, null));

        // Assert
        await Assert.That(cut.Markup).IsEqualTo($"{UserIcon} Player");
    }

    [Test]
    public async Task Render_WithPlatformIcon_DoesNotWrapIt()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, $"{PlayStationIcon} Player"));

        // Assert
        await Assert.That(cut.FindAll("span.tm-icon")).IsEmpty();
    }

    [Test]
    public async Task Render_WithAllowedLink_RendersAnchor()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters
            .Add(x => x.Text, "$l[https://example.com]Site$l")
            .Add(x => x.AllowLinks, true));

        // Assert
        await Assert.That(cut.Find("a").GetAttribute("href")).IsEqualTo("https://example.com");
    }

    [Test]
    public async Task Render_WithScriptLink_RendersTextOnly()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters
            .Add(x => x.Text, "$l[javascript:alert(1)]Site$l")
            .Add(x => x.AllowLinks, true));

        // Assert
        await Assert.That(cut.Markup).IsEqualTo("Site");
    }

    [Test]
    public async Task Render_WithDisallowedLink_RendersTextOnly()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, "$l[https://example.com]Site$l"));

        // Assert
        await Assert.That(cut.Markup).IsEqualTo("Site");
    }

    [Test]
    public async Task Render_WithAllowedManialink_RendersManiaplanetAnchor()
    {
        // Act
        var cut = Render<FormattedText>(parameters => parameters
            .Add(x => x.Text, "$h[bigbang1112]Profile$h")
            .Add(x => x.AllowManialinks, true));

        // Assert
        await Assert.That(cut.Find("a").GetAttribute("href")).IsEqualTo("maniaplanet:///:bigbang1112");
    }

    [Test]
    public void Render_WithChangedText_RendersNewText()
    {
        // Arrange
        var cut = Render<FormattedText>(parameters => parameters.Add(x => x.Text, "$f00Old"));

        // Act
        cut.Render(parameters => parameters.Add(x => x.Text, "$0f0New"));

        // Assert
        cut.MarkupMatches("""<span style="color:#0f0">New</span>""");
    }
}
