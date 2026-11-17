namespace TmEssentials.Benchmarks;

internal static class FormattedTextInputs
{
    public static readonly Dictionary<string, string> Cases = new()
    {
        ["Plain"] = "BigBang1112 is a nickname without any formatting",
        ["Colors"] = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01",
        ["ShortColors"] = "$abcC$fo$dl$befo$ar$aas",
        ["Styles"] = "$<B$wi$ng$oB$ia$tn$sg$g1$z112$>",
        ["Escapes"] = "$$B$$i$$g$$$B$$$a$$$$n$$$$g1112",
        ["Link"] = "$l[https://google.com]My nickname$l",
        ["Manialink"] = "$h[bigbang1112]BigBang1112$h",
        ["Mixed"] = "$<$000im$FC0G $fffKryw$> Checkout '$l[http://sm.mania-exchange.com/tracks/view/11880/]Elite - Paladin 2k14$l' ",
    };

    /// <summary>
    /// Edge cases that both implementations must handle the same way.
    /// </summary>
    public static readonly string[] EdgeCases =
    [
        "$l[a]x$l[b]y$l",
        "$l[pasdecrochetfermant",
        "$lYOUPI$l",
        "$[TAG$] 10$$$",
        "$qName$",
        "Line1\r\nLine2",
        "$l[u]A$<B$>C$l",
        "$<$<$<$<$<$<$<$<$<$f00A$>$>$>$>$>$>$>$>$>$>B",
        "$fY$f0Y$0f0Y$fA",
        "$l[$fb]$l[c]",
        "$tabc$<$tdef$>ghi$zjkl",
        "A$\nB$\r\nC",
        "$l[multi\nline]X$l",
        "$lmulti\nline$l",
        "$😀Name😀$fY",
        "$\uD800$fY",
        "End$\uD800",
    ];

    /// <summary>
    /// Throws if <paramref name="expected"/> and <paramref name="actual"/> produce different results for any input.
    /// </summary>
    public static void EnsureSameResults(Func<string, string> expected, Func<string, string> actual)
    {
        foreach (var input in Cases.Values.Concat(EdgeCases))
        {
            var expectedResult = expected(input);
            var actualResult = actual(input);

            if (expectedResult != actualResult)
            {
                throw new InvalidOperationException($"Results differ for '{input}': '{expectedResult}' vs '{actualResult}'.");
            }
        }
    }
}
