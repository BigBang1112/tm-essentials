using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class DeformatBenchmarks
{
    private string input = string.Empty;

    [Params("Plain", "Colors", "ShortColors", "Styles", "Escapes", "Link", "Manialink", "Mixed")]
    public string Case { get; set; } = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        FormattedTextInputs.EnsureSameResults(TextFormatter.Deformat, x => RegexTextFormatter.Deformat(x));
        FormattedTextInputs.EnsureSameResults(TextFormatter.Deformat, x => RegexValueMatchTextFormatter.Deformat(x));
        input = FormattedTextInputs.Cases[Case];
    }

    [Benchmark(Baseline = true)]
    public string Parser() => TextFormatter.Deformat(input);

    [Benchmark]
    public string Regex() => RegexTextFormatter.Deformat(input);

    [Benchmark]
    public string RegexValueMatch() => RegexValueMatchTextFormatter.Deformat(input);
}
