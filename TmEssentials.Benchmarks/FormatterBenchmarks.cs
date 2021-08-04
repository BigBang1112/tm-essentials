using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TmEssentials.Benchmarks
{
    public class FormatterBenchmarks
    {
        [Benchmark]
        public void DeformatLink()
        {
            Formatter.Deformat("$l[https://google.com]My nickname");
        }
    }
}
