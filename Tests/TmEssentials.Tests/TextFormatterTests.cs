using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials.Tests;

public class TextFormatterTests
{
    private const string AnsiRed = "\u001b[1;31m";
    private const string AnsiGreen = "\u001b[1;32m";
    private const string AnsiDefault = "\u001b[39m\u001b[22m";
    private static readonly Regex AnsiCodes = new("\u001b\\[[0-9;]*m");

    private const string KrazyColors ="$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";

    [Test]
    [Arguments("$l[https://google.com]My nickname$l", "My nickname")]
    [Arguments("$L[https://google.com]My nickname$l", "My nickname")]
    [Arguments("$l[superlongueURL//youpi]YOUPI$l", "YOUPI")]
    [Arguments("$lYOUPI$l", "YOUPI")]
    [Arguments("$l[pasdecrochetfermant", "pasdecrochetfermant")]
    public async Task Deformat_WithLink_ReturnsLinkText(string input, string expected)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(KrazyColors, "TMU.KrazyColors v0.1")]
    [Arguments("$abcC$f0o$del$befo$abr$aaas", "Colors")]
    [Arguments("$abcC$fo$dl$befo$ar$aas", "Colors")]
    [Arguments("$fYOUPI", "YOUPI")]
    [Arguments("$ff0YOUPI", "YOUPI")]
    public async Task Deformat_WithColorCodes_RemovesColorCodes(string input, string expected)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments("$h[bigbang1112]BigBang1112$h")]
    [Arguments("$p[bigbang1112]BigBang1112$p")]
    [Arguments("$H[bigbang1112]BigBang1112$H")]
    public async Task Deformat_WithManialink_ReturnsManialinkText(string input)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("BigBang1112");
    }

    [Test]
    public async Task Deformat_WithLinkCodeInsideLink_ClosesLinkAndKeepsBrackets()
    {
        // Arrange
        var input = "$l[a]x$l[b]y$l";

        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("x[b]y");
    }

    [Test]
    public async Task Deformat_WithEscapedBrackets_ReturnsBrackets()
    {
        // Arrange
        var input = "$[TAG$] Name";

        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("[TAG] Name");
    }

    [Test]
    [Arguments("$qName", "Name")]
    [Arguments("$ Name", "Name")]
    [Arguments("Name$", "Name")]
    public async Task Deformat_WithUnknownOrTrailingCode_RemovesCode(string input, string expected)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments("A$\nB", "AB")]
    [Arguments("A$\r\nB", "A\nB")]
    [Arguments("$l[multi\nline]X$l", "X")]
    [Arguments("$lmulti\nline$l", "multi\nline")]
    public async Task Deformat_WithLineBreaks_TreatsThemLikeOtherCharacters(string input, string expected)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments("$😀Name", "Name")]
    [Arguments("😀$fY", "😀Y")]
    public async Task Deformat_WithSurrogatePair_TreatsItAsOneCharacter(string input, string expected)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments("$\nB", "$\n")]
    [Arguments("$\rB", "$\r")]
    [Arguments("$l[multi\nline]X", "$l[multi\nline]")]
    [Arguments("$l[unclosed\nlink", "$l[unclosed\nlink")]
    [Arguments("$lX", "$l")]
    [Arguments("$f0Y", "$f0")]
    [Arguments("$😀X", "$😀")]
    [Arguments("X$", "$")]
    public async Task FormattingCodeRegexPattern_WithoutOptions_MatchesWholeCode(string input, string expected)
    {
        // Act
        var match = Regex.Match(input, TextFormatter.FormattingCodeRegexPattern);

        // Assert
        await Assert.That(match.Value).IsEqualTo(expected);
    }

    [Test]
    public async Task Deformat_WithCarriageReturn_RemovesCarriageReturn()
    {
        // Arrange
        var input = "Line1\r\nLine2";

        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("Line1\nLine2");
    }

    [Test]
    public async Task Deformat_WithNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => TextFormatter.Deformat(null!);

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task Deformat_WithStyleCodes_RemovesStyleCodes()
    {
        // Arrange
        var input = "$<B$wi$ng$oB$ia$tn$sg$g1$z112$>";

        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("BigBang1112");
    }

    [Test]
    public async Task Deformat_WithEscapedDollars_KeepsSingleDollars()
    {
        // Arrange
        var input = "$$B$$i$$g$$$B$$$a$$$$n$$$$g1112";

        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("$B$i$g$$$$n$$g1112");
    }

    [Test]
    public async Task Deformat_WithMaxReplacementCount_StopsAfterThatManyReplacements()
    {
        // Arrange
        var input = "$<B$wi$ng$oB$ia$tn$sg$g1$z112$>";

        // Act
        var actual = TextFormatter.Deformat(input, maxReplacementCount: 2);

        // Assert
        await Assert.That(actual).IsEqualTo("Bi$ng$oB$ia$tn$sg$g1$z112$>");
    }

    [Test]
    public async Task FormatAnsi_WithNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => TextFormatter.FormatAnsi(null!);

        // Assert
        await Assert.That(act).ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task FormatAnsi_WithColorCodes_ReturnsAnsiColoredText()
    {
        // Arrange
        var expected = "\u001b[1;31mT\u001b[1;31mM\u001b[1;31mU\u001b[0;31m.\u001b[0;35mK\u001b[0;35mr\u001b[0;34ma\u001b[0;34mz\u001b[1;34my\u001b[0;34mC\u001b[0;34mo\u001b[0;34ml\u001b[0;36mo\u001b[0;36mr\u001b[0;36ms\u001b[0;32m \u001b[0;32mv\u001b[0;32m0\u001b[0;32m.\u001b[0;32m1\u001b[39m\u001b[22m";

        // Act
        var actual = TextFormatter.FormatAnsi(KrazyColors);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task FormatAnsi_WithOneDigitColorCode_ReturnsAnsiColoredText()
    {
        // Act
        var actual = TextFormatter.FormatAnsi("$fY");

        // Assert
        await Assert.That(actual).IsEqualTo("\u001b[1;31mY\u001b[39m\u001b[22m");
    }

    [Test]
    public async Task FormatAnsi_WithEscapedDollar_KeepsSingleDollar()
    {
        // Act
        var actual = TextFormatter.FormatAnsi("10$$");

        // Assert
        await Assert.That(actual).IsEqualTo("10$\u001b[39m\u001b[22m");
    }

    [Test]
    [Arguments("$l[a]x$l[b]y$l")]
    [Arguments("$l[pasdecrochetfermant")]
    [Arguments("$lYOUPI$l")]
    [Arguments("$h[mx]Play$h")]
    [Arguments("$[TAG$] 10$$$")]
    [Arguments("$qName$")]
    [Arguments("Line1\r\nLine2")]
    [Arguments("$$B$$i$$g$$$B$$$a$$$$n$$$$g1112")]
    [Arguments(KrazyColors)]
    public async Task FormatAnsi_WithoutAnsiCodes_MatchesDeformat(string input)
    {
        // Arrange
        var expected = TextFormatter.Deformat(input);

        // Act
        var actual = AnsiCodes.Replace(TextFormatter.FormatAnsi(input), "");

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task FormatAnsi_WithScopeEnd_RestoresColorFromScopeStart()
    {
        // Act
        var actual = TextFormatter.FormatAnsi("$f00A$<$0f0B$>C");

        // Assert
        await Assert.That(actual).IsEqualTo($"{AnsiRed}A{AnsiGreen}B{AnsiRed}C{AnsiDefault}");
    }

    [Test]
    public async Task FormatAnsi_WithDefaultColorInsideScope_RestoresColorFromScopeStart()
    {
        // Act
        var actual = TextFormatter.FormatAnsi("$f00$<$0f0A$gB");

        // Assert
        await Assert.That(actual).IsEqualTo($"{AnsiRed}{AnsiGreen}A{AnsiRed}B{AnsiDefault}");
    }

    [Test]
    public async Task FormatAnsi_WithResetOutsideScope_ResetsToDefaultColor()
    {
        // Act
        var actual = TextFormatter.FormatAnsi("$f00A$zB");

        // Assert
        await Assert.That(actual).IsEqualTo($"{AnsiRed}A{AnsiDefault}B{AnsiDefault}");
    }

    [Test]
    public async Task FormatAnsi_WithUnmatchedScopeEnd_IgnoresIt()
    {
        // Act
        var actual = TextFormatter.FormatAnsi("$f00A$>B");

        // Assert
        await Assert.That(actual).IsEqualTo($"{AnsiRed}AB{AnsiDefault}");
    }

    [Test]
    [Arguments("$tabc$t def", "ABC def")]
    [Arguments("$tab$zc", "ABc")]
    [Arguments("$<$tab$>c", "ABc")]
    public async Task FormatAnsi_WithUppercase_UppercasesUntilToggledOrReset(string input, string expected)
    {
        // Act
        var actual = AnsiCodes.Replace(TextFormatter.FormatAnsi(input), "");

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task FormatHtml_WithNullOrEmpty_ReturnsEmpty(string? input)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input);

        // Assert
        await Assert.That(actual).IsEmpty();
    }

    [Test]
    [Arguments("$fY", "<span style=\"color:#f00\">Y</span>")]
    [Arguments("$f0Y", "<span style=\"color:#f00\">Y</span>")]
    [Arguments("$0f0Y", "<span style=\"color:#0f0\">Y</span>")]
    [Arguments("$fA", "")]
    [Arguments("$f0o", "<span style=\"color:#f00\">o</span>")]
    public async Task FormatHtml_WithColorCode_ColorsText(string input, string expected)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task FormatHtml_WithReset_ResetsColor()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$f00A$zB");

        // Assert
        await Assert.That(actual).IsEqualTo("<span style=\"color:#f00\">A</span>B");
    }

    [Test]
    public async Task FormatHtml_WithDefaultColorInsideScope_RestoresColorFromScopeStart()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$f00$<$0f0A$gB$>C");

        // Assert
        await Assert.That(actual).IsEqualTo("<span style=\"color:#0f0\">A</span><span style=\"color:#f00\">B</span><span style=\"color:#f00\">C</span>");
    }

    [Test]
    public async Task FormatHtml_WithNarrowTwice_KeepsNarrow()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$nA$nB$mC");

        // Assert
        await Assert.That(actual).IsEqualTo("<span style=\"letter-spacing:-0.05em\">A</span><span style=\"letter-spacing:-0.05em\">B</span>C");
    }

    [Test]
    public async Task FormatHtml_WithEscapedCharactersAndTrailingDollar_ReturnsLiterals()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$[TAG$] 10$$$");

        // Assert
        await Assert.That(actual).IsEqualTo("[TAG] 10$");
    }

    [Test]
    [Arguments("$l[https://example.com]Site$l after", "<a href=\"https://example.com\">Site</a> after")]
    [Arguments("$lhttps://example.com$l after", "<a href=\"https://example.com\">https://example.com</a> after")]
    public async Task FormatHtml_WithAllowedLink_ReturnsAnchor(string input, string expected)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input, allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task FormatHtml_WithDisallowedLink_ReturnsLinkTextOnly()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$l[https://example.com]Site$l after");

        // Assert
        await Assert.That(actual).IsEqualTo("Site after");
    }

    [Test]
    [Arguments("$h[mx]Play$h")]
    [Arguments("$p[mx]Play$p")]
    public async Task FormatHtml_WithAllowedManialink_ReturnsManiaplanetAnchor(string input)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input, allowManialinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo("<a href=\"maniaplanet:///:mx\">Play</a>");
    }

    [Test]
    public async Task FormatHtml_WithScopeInsideLink_KeepsLinkAfterScopeEnds()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$l[u]A$<B$>C$l", allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo("<a href=\"http://u\">A</a><a href=\"http://u\">B</a><a href=\"http://u\">C</a>");
    }

    [Test]
    public async Task FormatHtml_WithLinkWithoutBracketsEndingAtScopeEnd_EndsLinkText()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$<$lexample.com$> after", allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo("<a href=\"http://example.com\">example.com</a> after");
    }

    [Test]
    [Arguments("$l[http://example.com]Site$l", "<a href=\"http://example.com\">Site</a>")]
    [Arguments("$l[HTTPS://example.com]Site$l", "<a href=\"HTTPS://example.com\">Site</a>")]
    [Arguments("$l[www.example.com]Site$l", "<a href=\"http://www.example.com\">Site</a>")]
    [Arguments("$l[example.com/a:b]Site$l", "<a href=\"http://example.com/a:b\">Site</a>")]
    [Arguments("$l[//example.com]Site$l", "<a href=\"http:////example.com\">Site</a>")]
    public async Task FormatHtml_WithLink_HrefStartsWithHttp(string input, string expected)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input, allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments("$l[javascript:alert(1)]Site$l")]
    [Arguments("$l[JavaScript:alert(1)]Site$l")]
    [Arguments("$l[data:text/html,<script>alert(1)</script>]Site$l")]
    [Arguments("$l[vbscript:msgbox(1)]Site$l")]
    [Arguments("$l[mailto:a@example.com]Site$l")]
    [Arguments("$l[]Site$l")]
    public async Task FormatHtml_WithLinkOfOtherScheme_ReturnsLinkTextOnly(string input)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input, allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo("Site");
    }

    [Test]
    [Arguments("$l[ javascript:alert(1)]Site$l", "<a href=\"http:// javascript:alert(1)\">Site</a>")]
    [Arguments("$l[java\tscript:alert(1)]Site$l", "<a href=\"http://java\tscript:alert(1)\">Site</a>")]
    [Arguments("$l[java\nscript:alert(1)]Site$l", "<a href=\"http://java\nscript:alert(1)\">Site</a>")]
    [Arguments("$l[\u0001javascript:alert(1)]Site$l", "<a href=\"http://\u0001javascript:alert(1)\">Site</a>")]
    public async Task FormatHtml_WithObfuscatedScriptLink_PrefixesHttp(string input, string expected)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input, allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task FormatHtml_WithScriptLinkWithoutBrackets_ReturnsLinkTextOnly()
    {
        // Act
        var actual = TextFormatter.FormatHtml("$ljavascript:alert(1)$l", allowLinks: true);

        // Assert
        await Assert.That(actual).IsEqualTo("javascript:alert(1)");
    }

    [Test]
    [Arguments('', true)]
    [Arguments('', true)]
    [Arguments('', true)]
    [Arguments('', false)]
    [Arguments('', false)]
    [Arguments('', false)]
    [Arguments('A', false)]
    public async Task IsFontAwesomeIcon_WithCharacter_ReturnsWhetherInFontAwesome47Range(char c, bool expected)
    {
        // Act
        var actual = TextFormatter.IsFontAwesomeIcon(c);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    [Arguments(" Winner", "<span class=\"fa\"></span> Winner")]
    [Arguments("AB", "A<span class=\"fa\"></span>B")]
    [Arguments("$f00", "<span style=\"color:#f00\"><span class=\"fa\"></span></span>")]
    [Arguments("<>", "&lt;<span class=\"fa\"></span>&gt;")]
    [Arguments(" Player", " Player")]
    public async Task FormatHtml_WithIconCssClass_WrapsFontAwesomeIcons(string input, string expected)
    {
        // Act
        var actual = TextFormatter.FormatHtml(input, allowLinks: false, allowManialinks: false, iconCssClass: "fa");

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task FormatHtml_WithoutIconCssClass_DoesNotWrapIcons()
    {
        // Act
        var actual = TextFormatter.FormatHtml(" Winner");

        // Assert
        await Assert.That(actual).IsEqualTo(" Winner");
    }

    [Test]
    public async Task FormatHtml_WithIconCssClassContainingQuote_EncodesIt()
    {
        // Act
        var actual = TextFormatter.FormatHtml("", allowLinks: false, allowManialinks: false, iconCssClass: "a\"b");

        // Assert
        await Assert.That(actual).IsEqualTo("<span class=\"a&quot;b\"></span>");
    }

    [Test]
    public async Task AppendAnsiText_WithEmptyElement_AppendsNothing()
    {
        // Arrange
        var output = new StringBuilder();

        // Act
        TextFormatter.AppendAnsiText(output, "");

        // Assert
        await Assert.That(output.ToString()).IsEmpty();
    }

    [Test]
    public async Task AppendAnsiText_WithPlainText_AppendsTextUnchanged()
    {
        // Arrange
        var output = new StringBuilder();

        // Act
        TextFormatter.AppendAnsiText(output, "Hello");

        // Assert
        await Assert.That(output.ToString()).IsEqualTo("Hello");
    }

    [Test]
    public async Task AppendAnsiText_WithInvalidColorCode_AppendsNothing()
    {
        // Arrange
        var output = new StringBuilder();

        // Act
        TextFormatter.AppendAnsiText(output, "$G2");

        // Assert
        await Assert.That(output.ToString()).IsEmpty();
    }

    [Test]
    public async Task AppendAnsiText_WithBrightColorCode_AppendsBoldAnsiColor()
    {
        // Arrange
        var output = new StringBuilder();

        // Act
        TextFormatter.AppendAnsiText(output, "$FFF");

        // Assert
        await Assert.That(output.ToString()).IsEqualTo("\u001b[1;37m");
    }
}
