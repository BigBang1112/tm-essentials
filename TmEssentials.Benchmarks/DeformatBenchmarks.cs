﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class DeformatBenchmarks
{
    [Benchmark]
    public static void DeformatLinks()
    {
        TextFormatter.Deformat("$l[https://google.com]My nickname$l");
    }

    [Benchmark]
    public static void DeformatColors()
    {
        TextFormatter.Deformat("$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01");
    }

    [Benchmark]
    public static void DeformatColors2()
    {
        TextFormatter.Deformat("$abcC$f0o$del$befo$abr$aaas");
    }

    [Benchmark]
    public static void DeformatColors1()
    {
        TextFormatter.Deformat("$abcC$fo$dl$befo$ar$aas");
    }

    [Benchmark]
    public static void DeformatManialink()
    {
        TextFormatter.Deformat("$h[bigbang1112]BigBang1112$h");
    }

    [Benchmark]
    public static void DeformatSimpleFormatters()
    {
        TextFormatter.Deformat("$<B$wi$ng$oB$ia$tn$sg$g1$z112$>");
    }
}
