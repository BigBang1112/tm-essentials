using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class HtmlBenchmarks
{
    private string input = string.Empty;

    [Params("Plain", "Colors", "ShortColors", "Styles", "Escapes", "Link", "Manialink", "Mixed")]
    public string Case { get; set; } = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        input = FormattedTextInputs.Cases[Case];
    }

    [Benchmark]
    public string FormatHtml() => TextFormatter.FormatHtml(input, allowLinks: true, allowManialinks: true);
}
