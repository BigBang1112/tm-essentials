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
}
