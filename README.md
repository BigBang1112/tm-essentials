# Trackmania Essentials

[![Nuget](https://img.shields.io/nuget/v/TmEssentials?style=for-the-badge)](https://www.nuget.org/packages/TmEssentials/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/tm-essentials?include_prereleases&style=for-the-badge)](https://github.com/BigBang1112/tm-essentials/releases)

A super light-weight library that provides formatting features.

## Time formatting

`TimeSpanExtensions.ToMilliseconds()` - Total milliseconds as integer value with truncated ticks.

`TimeSpanExtensions.ToTmString()`

```cs
TimeSpan time = TimeSpan.FromSeconds(23.51);
string formattedTime = time.ToTmString();
Console.WriteLine(formattedTime);

// Output: 0:23.510
```

```cs
TimeSpan time = TimeSpan.FromSeconds(23.51);
string formattedTime = time.ToTmString(useHundredths: true);
Console.WriteLine(formattedTime);

// Output: 0:23.51
```

```cs
TimeSpan? noTime = null;
string formattedTime = noTime.ToTmString();
Console.WriteLine(formattedTime);

// Output: -:--.---
```

```cs
TimeSpan? noTime = null;
string formattedTime = noTime.ToTmString(useHundredths: true);
Console.WriteLine(formattedTime);

// Output: -:--.--
```

### Benchmarks

.NET 6 + .NET Standard 2.1 solution is based around usage of `Span` and `int.TryFormat()` with just a single allocation of `char[]`.

.NET Core 3.1 speed is about twice as slow, so it's recommended to target .NET 6 if possible.

```
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.102
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
```

|                                                 Method |      Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Allocated |
|------------------------------------------------------- |----------:|----------:|----------:|-------:|------:|----------:|
|                                 Null with milliseconds |  1.941 ns | 0.0375 ns | 0.0351 ns |      - |     - |         - |
|                                   Null with hundredths |  1.946 ns | 0.0373 ns | 0.0331 ns |      - |     - |         - |
|                             Negative milliseconds only | 37.075 ns | 0.6603 ns | 0.6176 ns | 0.0105 |     - |      88 B |
|                               Negative hundredths only | 37.524 ns | 0.6642 ns | 0.6213 ns | 0.0095 |     - |      80 B |
|                                        Hundredths only | 38.268 ns | 0.7200 ns | 0.6735 ns | 0.0095 |     - |      80 B |
|                                      Milliseconds only | 38.381 ns | 0.6198 ns | 0.5495 ns | 0.0095 |     - |      80 B |
|                 Negative seconds and milliseconds only | 44.973 ns | 0.5140 ns | 0.4808 ns | 0.0105 |     - |      88 B |
|                   Negative seconds and hundredths only | 46.631 ns | 0.7572 ns | 0.6323 ns | 0.0095 |     - |      80 B |
|                          Seconds and milliseconds only | 46.767 ns | 0.5210 ns | 0.4873 ns | 0.0095 |     - |      80 B |
|                            Seconds and hundredths only | 47.048 ns | 0.3230 ns | 0.2863 ns | 0.0095 |     - |      80 B |
|          2-digit minute, seconds and milliseconds only | 51.719 ns | 0.7748 ns | 0.6868 ns | 0.0105 |     - |      88 B |
|   Negative 2-digit minute, seconds and hundredths only | 52.419 ns | 0.7398 ns | 0.6920 ns | 0.0105 |     - |      88 B |
| Negative 2-digit minute, seconds and milliseconds only | 53.397 ns | 0.8768 ns | 0.7322 ns | 0.0114 |     - |      96 B |
|            2-digit minute, seconds and hundredths only | 54.050 ns | 1.0852 ns | 1.2918 ns | 0.0095 |     - |      80 B |
|            Hours, minutes, seconds and hundredths only | 54.284 ns | 1.0101 ns | 0.8954 ns | 0.0114 |     - |      96 B |
|          Hours, minutes, seconds and milliseconds only | 54.684 ns | 0.9962 ns | 0.8831 ns | 0.0114 |     - |      96 B |
|   Negative hours, minutes, seconds and hundredths only | 55.217 ns | 1.1260 ns | 0.9981 ns | 0.0114 |     - |      96 B |
|            1-digit minute, seconds and hundredths only | 55.904 ns | 0.5187 ns | 0.4598 ns | 0.0095 |     - |      80 B |
|          1-digit minute, seconds and milliseconds only | 56.184 ns | 1.0164 ns | 0.8487 ns | 0.0095 |     - |      80 B |
| Negative hours, minutes, seconds and milliseconds only | 56.666 ns | 0.6693 ns | 0.5589 ns | 0.0124 |     - |     104 B |
| Negative 1-digit minute, seconds and milliseconds only | 56.916 ns | 1.0004 ns | 0.8868 ns | 0.0105 |     - |      88 B |
|   Negative 1-digit minute, seconds and hundredths only | 58.220 ns | 0.3991 ns | 0.3333 ns | 0.0095 |     - |      80 B |
|           Fully-used negative TimeSpan with hundredths | 61.694 ns | 0.5250 ns | 0.4910 ns | 0.0134 |     - |     112 B |
|                    Fully-used TimeSpan with hundredths | 71.973 ns | 0.6981 ns | 0.6189 ns | 0.0124 |     - |     104 B |
|                  Fully-used TimeSpan with milliseconds | 73.025 ns | 1.1743 ns | 1.0985 ns | 0.0134 |     - |     112 B |
|         Fully-used negative TimeSpan with milliseconds | 75.944 ns | 1.3874 ns | 2.3181 ns | 0.0134 |     - |     112 B |

.NET Standard 2.0 or any other lower solution is based around usage of `StringBuilder`, which is an additional heap allocation.

Here is an example of .NET Core 2.1, one of the last versions that aren't supported by .NET Standard 2.1:

```
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.102
  [Host]     : .NET Core 2.1.30 (CoreCLR 4.6.30411.01, CoreFX 4.6.30411.02), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 2.1.30 (CoreCLR 4.6.30411.01, CoreFX 4.6.30411.02), X64 RyuJIT
```

|                                                 Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Allocated |
|------------------------------------------------------- |-----------:|----------:|----------:|-------:|------:|----------:|
|                                 Null with milliseconds |   3.116 ns | 0.0126 ns | 0.0105 ns |      - |     - |         - |
|                                   Null with hundredths |   3.160 ns | 0.0425 ns | 0.0355 ns |      - |     - |         - |
|          2-digit minute, seconds and milliseconds only | 109.078 ns | 0.7196 ns | 0.6009 ns | 0.1159 |     - |     152 B |
|                          Seconds and milliseconds only | 110.899 ns | 0.5687 ns | 0.5041 ns | 0.1159 |     - |     152 B |
|            2-digit minute, seconds and hundredths only | 119.764 ns | 0.5150 ns | 0.4301 ns | 0.1159 |     - |     152 B |
|                            Seconds and hundredths only | 121.653 ns | 1.1993 ns | 1.0015 ns | 0.1097 |     - |     144 B |
|          1-digit minute, seconds and milliseconds only | 131.119 ns | 1.3792 ns | 1.2901 ns | 0.1159 |     - |     152 B |
|                                      Milliseconds only | 132.801 ns | 0.7855 ns | 0.6963 ns | 0.1159 |     - |     152 B |
|                 Negative seconds and milliseconds only | 135.784 ns | 0.8043 ns | 0.7130 ns | 0.1159 |     - |     152 B |
|            1-digit minute, seconds and hundredths only | 140.294 ns | 0.9030 ns | 0.8005 ns | 0.1097 |     - |     144 B |
| Negative 2-digit minute, seconds and milliseconds only | 141.199 ns | 2.7401 ns | 3.0457 ns | 0.1159 |     - |     152 B |
|                                        Hundredths only | 142.860 ns | 0.8810 ns | 0.6878 ns | 0.1097 |     - |     144 B |
|                   Negative seconds and hundredths only | 155.710 ns | 1.0900 ns | 1.0196 ns | 0.1159 |     - |     152 B |
|   Negative 2-digit minute, seconds and hundredths only | 158.877 ns | 0.7833 ns | 0.6944 ns | 0.1159 |     - |     152 B |
|                             Negative milliseconds only | 160.716 ns | 1.1390 ns | 1.0097 ns | 0.1159 |     - |     152 B |
| Negative 1-digit minute, seconds and milliseconds only | 161.800 ns | 1.8859 ns | 1.7640 ns | 0.1159 |     - |     152 B |
|                               Negative hundredths only | 179.311 ns | 1.1194 ns | 0.9923 ns | 0.1159 |     - |     152 B |
|   Negative 1-digit minute, seconds and hundredths only | 181.251 ns | 1.8244 ns | 1.5235 ns | 0.1159 |     - |     152 B |
|          Hours, minutes, seconds and milliseconds only | 228.599 ns | 2.5959 ns | 2.3012 ns | 0.1464 |     - |     192 B |
|            Hours, minutes, seconds and hundredths only | 235.763 ns | 1.1991 ns | 1.1216 ns | 0.1402 |     - |     184 B |
|                    Fully-used TimeSpan with hundredths | 247.080 ns | 1.4586 ns | 1.3644 ns | 0.1707 |     - |     224 B |
| Negative hours, minutes, seconds and milliseconds only | 255.434 ns | 2.9534 ns | 2.7626 ns | 0.1464 |     - |     192 B |
|         Fully-used negative TimeSpan with milliseconds | 269.613 ns | 5.1582 ns | 6.1405 ns | 0.1707 |     - |     224 B |
|                  Fully-used TimeSpan with milliseconds | 286.841 ns | 5.3611 ns | 5.2653 ns | 0.1707 |     - |     224 B |
|   Negative hours, minutes, seconds and hundredths only | 298.832 ns | 4.6682 ns | 4.1382 ns | 0.1464 |     - |     192 B |
|           Fully-used negative TimeSpan with hundredths | 340.673 ns | 6.6231 ns | 9.4986 ns | 0.1707 |     - |     224 B |

## Text formatting

Currently only deformat.

Credits to [reaby](https://github.com/reaby) for the Regex pattern.

`TextFormatter.Deformat()`

```cs
var formatted = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";
var deformatted = TextFormatter.Deformat(formatted);
Console.WriteLine(deformatted);

// Output: TMU.KrazyColors v0.1
```

## .NET Standard 2+

`HttpClientExtensions.HeadAsync()`

Implements the HEAD request into a simple method. HEAD request is useful for getting the last modified date.