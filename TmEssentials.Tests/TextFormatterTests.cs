using System.Text;

namespace TmEssentials.Tests;

public class TextFormatterTests
{
    private const string KrazyColors = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";

    [Test]
    [Arguments("$l[https://google.com]My nickname$l")]
    [Arguments("$L[https://google.com]My nickname$l")]
    public async Task Deformat_WithLink_ReturnsLinkText(string input)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("My nickname");
    }

    [Test]
    [Arguments(KrazyColors, "TMU.KrazyColors v0.1")]
    [Arguments("$abcC$f0o$del$befo$abr$aaas", "Colors")]
    [Arguments("$abcC$fo$dl$befo$ar$aas", "Colors")]
    public async Task Deformat_WithColorCodes_RemovesColorCodes(string input, string expected)
    {
        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Deformat_WithManialink_ReturnsManialinkText()
    {
        // Arrange
        var input = "$h[bigbang1112]BigBang1112$h";

        // Act
        var actual = TextFormatter.Deformat(input);

        // Assert
        await Assert.That(actual).IsEqualTo("BigBang1112");
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
