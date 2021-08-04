using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TmEssentials.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class FormatterBenchmarks
    {
        [Benchmark]
        public void DeformatLinks()
        {
            Formatter.Deformat("$l[https://google.com]My nickname$l");
        }

        [Benchmark]
        public void DeformatColors()
        {
            Formatter.Deformat("$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01");
        }

        [Benchmark]
        public void DeformatColors2()
        {
            Formatter.Deformat("$abcC$f0o$del$befo$abr$aaas");
        }

        [Benchmark]
        public void DeformatColors1()
        {
            Formatter.Deformat("$abcC$fo$dl$befo$ar$aas");
        }

        [Benchmark]
        public void DeformatManialink()
        {
            Formatter.Deformat("$h[bigbang1112]BigBang1112$h");
        }

        [Benchmark]
        public void DeformatSimpleFormatters()
        {
            Formatter.Deformat("$<B$wi$ng$oB$ia$tn$sg$g1$z112$>");
        }
    }
}
