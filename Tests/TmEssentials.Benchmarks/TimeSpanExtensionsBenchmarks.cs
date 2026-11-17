using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;

namespace TmEssentials.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class TimeSpanExtensionsBenchmarks
{
    private static readonly TimeSpan? nullTime = null;
    private static readonly TimeSpan fff = TimeSpan.FromMilliseconds(123);
    private static readonly TimeSpan fffNegative = -fff;
    private static readonly TimeSpan ssfff = TimeSpan.FromSeconds(12.345);
    private static readonly TimeSpan ssfffNegative = -ssfff;
    private static readonly TimeSpan mssfff = TimeSpan.FromMinutes(1.234);
    private static readonly TimeSpan mssfffNegative = -mssfff;
    private static readonly TimeSpan mmssfff = TimeSpan.FromMinutes(12.345);
    private static readonly TimeSpan mmssfffNegative = -mmssfff;
    private static readonly TimeSpan hmmssfff = TimeSpan.FromHours(12.345);
    private static readonly TimeSpan hmmssfffNegative = -hmmssfff;
    private static readonly TimeSpan fullTime = new(1, 15, 56, 43, 165);
    private static readonly TimeSpan fullNegativeTime = -fullTime;

    [Benchmark(Description = "Null with milliseconds")]
    public string FormatNullTime()
    {
        return nullTime.ToTmString();
    }

    [Benchmark(Description = "Null with hundredths")]
    public string FormatNullTimeHundredths()
    {
        return nullTime.ToTmString(true);
    }

    [Benchmark(Description = "Milliseconds only")]
    public string FormatFFF()
    {
        return fff.ToTmString();
    }

    [Benchmark(Description = "Hundredths only")]
    public string FormatFF()
    {
        return fff.ToTmString(true);
    }

    [Benchmark(Description = "Seconds and milliseconds only")]
    public string FormatSSFFF()
    {
        return ssfff.ToTmString();
    }

    [Benchmark(Description = "Seconds and hundredths only")]
    public string FormatSSFF()
    {
        return ssfff.ToTmString(true);
    }

    [Benchmark(Description = "1-digit minute, seconds and milliseconds only")]
    public string FormatMSSFFF()
    {
        return mssfff.ToTmString();
    }

    [Benchmark(Description = "1-digit minute, seconds and hundredths only")]
    public string FormatMSSFF()
    {
        return mssfff.ToTmString(true);
    }

    [Benchmark(Description = "2-digit minute, seconds and milliseconds only")]
    public string FormatMMSSFFF()
    {
        return mmssfff.ToTmString();
    }

    [Benchmark(Description = "2-digit minute, seconds and hundredths only")]
    public string FormatMMSSFF()
    {
        return mmssfff.ToTmString(true);
    }

    [Benchmark(Description = "Hours, minutes, seconds and milliseconds only")]
    public string FormatHMMSSFFF()
    {
        return hmmssfff.ToTmString();
    }

    [Benchmark(Description = "Hours, minutes, seconds and hundredths only")]
    public string FormatHMMSSFF()
    {
        return hmmssfff.ToTmString(true);
    }

    [Benchmark(Description = "Fully-used TimeSpan with milliseconds")]
    public string FormatFullTime()
    {
        return fullTime.ToTmString();
    }

    [Benchmark(Description = "Fully-used TimeSpan with hundredths")]
    public string FormatFullTimeHundredths()
    {
        return fullTime.ToTmString(true);
    }

    [Benchmark(Description = "Negative milliseconds only")]
    public string FormatNegativeFFF()
    {
        return fffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative hundredths only")]
    public string FormatNegativeFF()
    {
        return fffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative seconds and milliseconds only")]
    public string FormatNegativeSSFFF()
    {
        return ssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative seconds and hundredths only")]
    public string FormatNegativeSSFF()
    {
        return ssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative 1-digit minute, seconds and milliseconds only")]
    public string FormatNegativeMSSFFF()
    {
        return mssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative 1-digit minute, seconds and hundredths only")]
    public string FormatNegativeMSSFF()
    {
        return mssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative 2-digit minute, seconds and milliseconds only")]
    public string FormatNegativeMMSSFFF()
    {
        return mmssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative 2-digit minute, seconds and hundredths only")]
    public string FormatNegativeMMSSFF()
    {
        return mmssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative hours, minutes, seconds and milliseconds only")]
    public string FormatNegativeHMMSSFFF()
    {
        return hmmssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative hours, minutes, seconds and hundredths only")]
    public string FormatNegativeHMMSSFF()
    {
        return hmmssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Fully-used negative TimeSpan with milliseconds")]
    public string FormatFullNegativeTime()
    {
        return fullNegativeTime.ToTmString();
    }

    [Benchmark(Description = "Fully-used negative TimeSpan with hundredths")]
    public string FormatFullNegativeTimeHundredths()
    {
        return fullNegativeTime.ToTmString(true);
    }
}
