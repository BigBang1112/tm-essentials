# Trackmania Essentials

[![Nuget](https://img.shields.io/nuget/v/TmEssentials?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/TmEssentials/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/tm-essentials?include_prereleases&style=for-the-badge&logo=github)](https://github.com/BigBang1112/tm-essentials/releases)
[![Code Coverage](https://img.shields.io/badge/Code%20Coverage-87%25-success?style=for-the-badge)](https://github.com/BigBang1112/tm-essentials)

A super light-weight library that provides formatting features.

## Time formatting

### Two new structs: `TimeInt32` and `TimeSingle`

- Storage size is half smaller than with `TimeSpan`.
- Reduced floating point errors, previously caused by `double`/`float` casting.
- Struct methods are nearly the same to `TimeSpan`.
- Consistency is partially covered by the `ITime` interface.
- Implicitly modified `ToString()` to use a Trackmania-familiar format.
- `ToTmString()` extension methods for consistency reasons (due to nullable value types).
- Parse methods are available.
- Supports `TypeConverter` for input binding scenarios.

Example usage of `TimeInt32`:

```cs
TimeInt32 time = new TimeInt32(TotalMilliseconds: 36217);
string formattedTime = time.ToTmString(); // ToString() acts the same
Console.WriteLine(formattedTime); // Output: 0:36.217

string formattedTimeUsingHundredths = time.ToTmString(useHundredths: true);
Console.WriteLine(formattedTimeUsingHundredths); // Output: 0:36.21

string formattedTimeUsingApostrophe = time.ToTmString(useApostrophe: true);
Console.WriteLine(formattedTimeUsingApostrophe); // Output: 0'36''217

TimeInt32? noTime = null;
string formattedNoTime = noTime.ToTmString();
Console.WriteLine(formattedNoTime); // Output: -:--.---

string formattedNoTimeUsingHundredths = noTime.ToTmString(useHundredths: true);
Console.WriteLine(formattedNoTimeUsingHundredths); // Output: -:--.--

string formattedNoTimeUsingApostrophe = noTime.ToTmString(useApostrophe: true);
Console.WriteLine(formattedNoTimeUsingApostrophe); // Output: -'--''---
```

Example usage of `TimeSingle`:

```cs
TimeSingle time = new TimeSingle(TotalSeconds: 23.51f);
string formattedTime = time.ToTmString();
Console.WriteLine(formattedTime); // Output: 0:23.510

string formattedTimeUsingHundredths = time.ToTmString(useHundredths: true);
Console.WriteLine(formattedTimeUsingHundredths); // Output: 0:23.51

string formattedTimeUsingApostrophe = time.ToTmString(useApostrophe: true);
Console.WriteLine(formattedTimeUsingApostrophe); // Output: 0'23''510

TimeSingle? noTime = null;
string formattedNoTime = noTime.ToTmString();
Console.WriteLine(formattedNoTime); // Output: -:--.---

string formattedNoTimeUsingHundredths = noTime.ToTmString(useHundredths: true);
Console.WriteLine(formattedNoTimeUsingHundredths); // Output: -:--.--

string formattedNoTimeUsingApostrophe = noTime.ToTmString(useApostrophe: true);
Console.WriteLine(formattedNoTimeUsingApostrophe); // Output: -'--''---
```

### Formatting extensions for TimeSpan

- Same formatting features like `TimeInt32`/`TimeSingle` has but on `TimeSpan`.
- `TimeSpanExtensions.ToMilliseconds()` - Total milliseconds as integer value with truncated ticks.

## Text formatting

Currently only deformat.

Credits to [Tom](https://github.com/ThaumicTom) and [Stefan Baumann](https://github.com/stefan-baumann) for their effective Regex pattern.

`TextFormatter.Deformat()`

```cs
var formatted = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";
var deformatted = TextFormatter.Deformat(formatted);
Console.WriteLine(deformatted);

// Output: TMU.KrazyColors v0.1
```

## ANSI formatting of Trackmania string

Credits to [reaby](https://github.com/reaby) for the solution.

```cs
var formatted = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";
var ansiFormatted = TextFormatter.FormatAnsi(formatted);

// Rough output: \\u001b[1;31mT\\u001b[1;31mM\\u001b[1;31mU\\u001b[0;31m.\\u001b[0;35mK\\u001b[0;35mr\\u001b[0;34ma\\u001b[0;34mz\\u001b[1;34my\\u001b[0;34mC\\u001b[0;34mo\\u001b[0;34ml\\u001b[0;36mo\\u001b[0;36mr\\u001b[0;36ms\\u001b[0;32m \\u001b[0;32mv\\u001b[0;32m0\\u001b[0;32m.\\u001b[0;32m1\\u001b[39m\\u001b[22m
```

[image soon]

## Accounts formatting

Currently only has a ***very*** efficient account ID/login converter.

`AccountUtils.ToAccountId()`

```cs
AccountUtils.ToAccountId("v89i_w-eQKq5JBG5xwuKCQ") // "bfcf62ff-0f9e-40aa-b924-11b9c70b8a09"
```

`AccountUtils.ToLogin()`

```cs
AccountUtils.ToLogin(Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09")) // "v89i_w-eQKq5JBG5xwuKCQ"
```

## Map UID generation

An approximate implementation of 27-character map UID generation.

`MapUtils.GenerateMapUid()`

## .NET Standard 2+

`HttpClientExtensions.HeadAsync(string requestUri, CancellationToken cancellationToken = default)`

Implements the HEAD request into a simple method. HEAD request is useful for getting the last modified date.

## Benchmarks

The library tries to be as effective as possible to an absurd level with using managed code only.

```
BenchmarkDotNet v0.13.11, Windows 11 (10.0.22621.2861/22H2/2022Update/SunValley2)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                                                   | Mean       | Error     | StdDev    | Median     | Gen0   | Allocated |
|--------------------------------------------------------- |-----------:|----------:|----------:|-----------:|-------:|----------:|
| 'Null with hundredths'                                   |  0.2379 ns | 0.0154 ns | 0.0144 ns |  0.2363 ns |      - |         - |
| 'Null with milliseconds'                                 |  1.9493 ns | 0.0123 ns | 0.0103 ns |  1.9540 ns |      - |         - |
| 'Seconds and milliseconds only'                          | 30.5358 ns | 0.6123 ns | 1.0884 ns | 29.9976 ns | 0.0048 |      40 B |
| '1-digit minute, seconds and milliseconds only'          | 30.5601 ns | 0.6134 ns | 0.5122 ns | 30.4337 ns | 0.0048 |      40 B |
| '2-digit minute, seconds and hundredths only'            | 30.6992 ns | 0.2823 ns | 0.2358 ns | 30.6543 ns | 0.0048 |      40 B |
| 'Milliseconds only'                                      | 30.9590 ns | 0.6381 ns | 0.8734 ns | 30.9244 ns | 0.0048 |      40 B |
| 'Hundredths only'                                        | 31.0166 ns | 0.6434 ns | 1.1098 ns | 30.7131 ns | 0.0048 |      40 B |
| 'Negative seconds and hundredths only'                   | 31.5512 ns | 0.6633 ns | 0.7896 ns | 31.3869 ns | 0.0048 |      40 B |
| '1-digit minute, seconds and hundredths only'            | 31.6155 ns | 0.5243 ns | 0.5149 ns | 31.6675 ns | 0.0048 |      40 B |
| 'Seconds and hundredths only'                            | 31.8579 ns | 0.6069 ns | 1.0140 ns | 31.6990 ns | 0.0048 |      40 B |
| 'Negative milliseconds only'                             | 31.9256 ns | 0.3962 ns | 0.3513 ns | 31.8951 ns | 0.0048 |      40 B |
| '2-digit minute, seconds and milliseconds only'          | 31.9811 ns | 0.6515 ns | 0.8698 ns | 31.6213 ns | 0.0048 |      40 B |
| 'Negative hundredths only'                               | 31.9918 ns | 0.6653 ns | 0.7395 ns | 31.8978 ns | 0.0048 |      40 B |
| 'Negative 1-digit minute, seconds and milliseconds only' | 32.0097 ns | 0.6535 ns | 0.8498 ns | 31.7147 ns | 0.0048 |      40 B |
| 'Negative 2-digit minute, seconds and milliseconds only' | 32.2230 ns | 0.6680 ns | 0.7424 ns | 32.1323 ns | 0.0057 |      48 B |
| 'Negative 1-digit minute, seconds and hundredths only'   | 32.3645 ns | 0.6732 ns | 0.8014 ns | 32.2189 ns | 0.0048 |      40 B |
| 'Negative seconds and milliseconds only'                 | 33.3655 ns | 0.6869 ns | 1.2902 ns | 33.0008 ns | 0.0048 |      40 B |
| 'Negative 2-digit minute, seconds and hundredths only'   | 33.8375 ns | 0.4832 ns | 0.4284 ns | 33.8595 ns | 0.0048 |      40 B |
| 'Hours, minutes, seconds and hundredths only'            | 38.0449 ns | 0.7887 ns | 0.6991 ns | 37.9283 ns | 0.0057 |      48 B |
| 'Hours, minutes, seconds and milliseconds only'          | 38.2250 ns | 0.7767 ns | 0.9822 ns | 38.2305 ns | 0.0057 |      48 B |
| 'Negative hours, minutes, seconds and milliseconds only' | 38.2533 ns | 0.4994 ns | 0.4170 ns | 38.3381 ns | 0.0057 |      48 B |
| 'Negative hours, minutes, seconds and hundredths only'   | 38.6611 ns | 0.5701 ns | 0.5333 ns | 38.5405 ns | 0.0057 |      48 B |
| 'Fully-used TimeSpan with hundredths'                    | 40.3112 ns | 0.4293 ns | 0.3806 ns | 40.2566 ns | 0.0057 |      48 B |
| 'Fully-used TimeSpan with milliseconds'                  | 40.4784 ns | 0.8199 ns | 0.8052 ns | 40.3464 ns | 0.0067 |      56 B |
| 'Fully-used negative TimeSpan with milliseconds'         | 40.8930 ns | 0.4993 ns | 0.4670 ns | 40.7865 ns | 0.0067 |      56 B |
| 'Fully-used negative TimeSpan with hundredths'           | 41.2768 ns | 0.6247 ns | 0.5843 ns | 41.3455 ns | 0.0067 |      56 B |
```
