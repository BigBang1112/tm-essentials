using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class TimeFormatBenchmarks
{
    private TimeInt32 timeInt32;
    private TimeSingle timeSingle;

    [Params(123, 83_456, -83_456, 3_723_456)]
    public int Milliseconds { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        timeInt32 = new TimeInt32(Milliseconds);
        timeSingle = new TimeSingle(Milliseconds / 1000f);
    }

    [Benchmark]
    public string TimeInt32ToString() => timeInt32.ToString();

    [Benchmark]
    public string TimeSingleToString() => timeSingle.ToString();

    [Benchmark]
    public string TimeInt32ToStringHundredths() => timeInt32.ToString(useHundredths: true);
}
