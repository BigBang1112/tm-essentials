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
    public static void FormatNullTime()
    {
        nullTime.ToTmString();
    }

    [Benchmark(Description = "Null with hundredths")]
    public static void FormatNullTimeHundredths()
    {
        nullTime.ToTmString(true);
    }

    [Benchmark(Description = "Milliseconds only")]
    public static void FormatFFF()
    {
        fff.ToTmString();
    }

    [Benchmark(Description = "Hundredths only")]
    public static void FormatFF()
    {
        fff.ToTmString(true);
    }

    [Benchmark(Description = "Seconds and milliseconds only")]
    public static void FormatSSFFF()
    {
        ssfff.ToTmString();
    }

    [Benchmark(Description = "Seconds and hundredths only")]
    public static void FormatSSFF()
    {
        ssfff.ToTmString(true);
    }

    [Benchmark(Description = "1-digit minute, seconds and milliseconds only")]
    public static void FormatMSSFFF()
    {
        mssfff.ToTmString();
    }

    [Benchmark(Description = "1-digit minute, seconds and hundredths only")]
    public static void FormatMSSFF()
    {
        mssfff.ToTmString(true);
    }

    [Benchmark(Description = "2-digit minute, seconds and milliseconds only")]
    public static void FormatMMSSFFF()
    {
        mmssfff.ToTmString();
    }

    [Benchmark(Description = "2-digit minute, seconds and hundredths only")]
    public static void FormatMMSSFF()
    {
        mmssfff.ToTmString(true);
    }

    [Benchmark(Description = "Hours, minutes, seconds and milliseconds only")]
    public static void FormatHMMSSFFF()
    {
        hmmssfff.ToTmString();
    }

    [Benchmark(Description = "Hours, minutes, seconds and hundredths only")]
    public static void FormatHMMSSFF()
    {
        hmmssfff.ToTmString(true);
    }

    [Benchmark(Description = "Fully-used TimeSpan with milliseconds")]
    public static void FormatFullTime()
    {
        fullTime.ToTmString();
    }

    [Benchmark(Description = "Fully-used TimeSpan with hundredths")]
    public static void FormatFullTimeHundredths()
    {
        fullTime.ToTmString(true);
    }

    [Benchmark(Description = "Negative milliseconds only")]
    public static void FormatNegativeFFF()
    {
        fffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative hundredths only")]
    public static void FormatNegativeFF()
    {
        fffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative seconds and milliseconds only")]
    public static void FormatNegativeSSFFF()
    {
        ssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative seconds and hundredths only")]
    public static void FormatNegativeSSFF()
    {
        ssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative 1-digit minute, seconds and milliseconds only")]
    public static void FormatNegativeMSSFFF()
    {
        mssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative 1-digit minute, seconds and hundredths only")]
    public static void FormatNegativeMSSFF()
    {
        mssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative 2-digit minute, seconds and milliseconds only")]
    public static void FormatNegativeMMSSFFF()
    {
        mmssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative 2-digit minute, seconds and hundredths only")]
    public static void FormatNegativeMMSSFF()
    {
        mmssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Negative hours, minutes, seconds and milliseconds only")]
    public static void FormatNegativeHMMSSFFF()
    {
        hmmssfffNegative.ToTmString();
    }

    [Benchmark(Description = "Negative hours, minutes, seconds and hundredths only")]
    public static void FormatNegativeHMMSSFF()
    {
        hmmssfffNegative.ToTmString(true);
    }

    [Benchmark(Description = "Fully-used negative TimeSpan with milliseconds")]
    public static void FormatFullNegativeTime()
    {
        fullNegativeTime.ToTmString();
    }

    [Benchmark(Description = "Fully-used negative TimeSpan with hundredths")]
    public static void FormatFullNegativeTimeHundredths()
    {
        fullNegativeTime.ToTmString(true);
    }
}
