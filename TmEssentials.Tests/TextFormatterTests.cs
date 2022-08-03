using System;
using System.Text;
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
        var expectedBase64 = "G1sxOzMxbVQbWzE7MzFtTRtbMTszMW1VG1swOzMxbS4bWzA7MzVtSxtbMDszNW1yG1swOzM0bWEbWzA7MzRtehtbMTszNG15G1swOzM0bUMbWzA7MzRtbxtbMDszNG1sG1swOzM2bW8bWzA7MzZtchtbMDszNm1zG1swOzMybSAbWzA7MzJtdhtbMDszMm0wG1swOzMybS4bWzA7MzJtMRtbMzltG1syMm0=";

        var ansi = TextFormatter.FormatAnsi("$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01");
        
        var actualBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(ansi));

        Assert.Equal(expectedBase64, actualBase64);
    }
}
