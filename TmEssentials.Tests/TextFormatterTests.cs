using Xunit;

namespace TmEssentials.Tests;

public class TextFormatterTests
{
    [Fact]
    public void Deformat_Links()
    {
        var expected = "My nickname";

        var actual = TextFormatter.Deformat("$l[https://google.com]My nickname$l");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deformat_Colors()
    {
        var expected = "TMU.KrazyColors v0.1";

        var actual = TextFormatter.Deformat("$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deformat_Colors2()
    {
        var expected = "Colors";

        var actual = TextFormatter.Deformat("$abcC$f0o$del$befo$abr$aaas");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deformat_Colors1()
    {
        var expected = "Colors";

        var actual = TextFormatter.Deformat("$abcC$fo$dl$befo$ar$aas");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deformat_Manialink()
    {
        var expected = "BigBang1112";

        var actual = TextFormatter.Deformat("$h[bigbang1112]BigBang1112$h");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deformat_SimpleFormatters()
    {
        var expected = "BigBang1112";

        var actual = TextFormatter.Deformat("$<B$wi$ng$oB$ia$tn$sg$g1$z112$>");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deformat_DoubleDollar()
    {
        var expected = "$B$i$g$$$$n$$g1112";

        var actual = TextFormatter.Deformat("$$B$$i$$g$$$B$$$a$$$$n$$$$g1112");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatAnsi_FormatsCorrectly()
    {
        var expected = "\\u001b[1;31mT\\u001b[1;31mM\\u001b[1;31mU\\u001b[0;31m.\\u001b[0;35mK\\u001b[0;35mr\\u001b[0;34ma\\u001b[0;34mz\\u001b[1;34my\\u001b[0;34mC\\u001b[0;34mo\\u001b[0;34ml\\u001b[0;36mo\\u001b[0;36mr\\u001b[0;36ms\\u001b[0;32m \\u001b[0;32mv\\u001b[0;32m0\\u001b[0;32m.\\u001b[0;32m1\\u001b[39m\\u001b[22m";

        var actual = TextFormatter.FormatAnsi("$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01")
            .Replace("\x1B", "\\u001b"); // purely testing purposes

        Assert.Equal(expected, actual);
    }
}
