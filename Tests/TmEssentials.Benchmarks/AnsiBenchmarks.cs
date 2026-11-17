using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class AnsiBenchmarks
{
    private string input = string.Empty;

    [Params("Plain", "Colors", "ShortColors", "Styles", "Escapes", "Link", "Manialink", "Mixed")]
    public string Case { get; set; } = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        FormattedTextInputs.EnsureSameResults(TextFormatter.FormatAnsi, RegexTextFormatter.FormatAnsi);
        FormattedTextInputs.EnsureSameResults(TextFormatter.FormatAnsi, RegexValueMatchTextFormatter.FormatAnsi);
        input = FormattedTextInputs.Cases[Case];
    }

    [Benchmark(Baseline = true)]
    public string Parser() => TextFormatter.FormatAnsi(input);

    [Benchmark]
    public string Regex() => RegexTextFormatter.FormatAnsi(input);

    [Benchmark]
    public string RegexValueMatch() => RegexValueMatchTextFormatter.FormatAnsi(input);
}
