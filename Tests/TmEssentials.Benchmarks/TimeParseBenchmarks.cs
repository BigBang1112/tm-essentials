using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class TimeParseBenchmarks
{
    /// <summary>
    /// The formats that the time types accept, for comparing with <see cref="TimeSpan.TryParseExact(string, string[], IFormatProvider, out TimeSpan)"/>.
    /// </summary>
    private static readonly string[] TimeFormats =
    [
        @"FFFFFFF",
        @"s\.FFFFFFF",
        @"m\:ss\.FFFFFFF",
        @"h\:mm\:ss\.FFFFFFF",
        @"d\:hh\:mm\:ss\.FFFFFFF",
        @"\-FFFFFFF",
        @"\-s\.FFFFFFF",
        @"\-m\:ss\.FFFFFFF",
        @"\-h\:mm\:ss\.FFFFFFF",
        @"\-d\:hh\:mm\:ss\.FFFFFFF"
    ];

    [Params("0.123", "12.345", "1:23.456", "-1:23.456", "1:02:03.456", "1:02:03:04.567", "Invalid")]
    public string Input { get; set; } = string.Empty;

    [Benchmark(Baseline = true)]
    public bool TimeSpanTryParseExact() => TimeSpan.TryParseExact(Input, TimeFormats, null, out _);

    [Benchmark]
    public bool TimeInt32TryParse() => TimeInt32.TryParse(Input, out _);

    [Benchmark]
    public bool TimeSingleTryParse() => TimeSingle.TryParse(Input, out _);
}
