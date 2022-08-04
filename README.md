# Trackmania Essentials

[![Nuget](https://img.shields.io/nuget/v/TmEssentials?style=for-the-badge)](https://www.nuget.org/packages/TmEssentials/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/tm-essentials?include_prereleases&style=for-the-badge)](https://github.com/BigBang1112/tm-essentials/releases)

A super light-weight library that provides formatting features.

## Time formatting

### Two new structs: `TimeInt32` and `TimeSingle`

- Storage size is half smaller than with `TimeSpan`.
- Reduced floating point errors, previously caused by `double`/`float` casting.
- Struct methods are nearly the same to `TimeSpan`.
- Consistency is partially covered by the `ITime` interface.
- Implicitly modified `ToString()` to use a Trackmania-familiar format.
- `ToTmString()` extension methods for consistency reasons (due to nullable value types).

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

## .NET Standard 2+

`HttpClientExtensions.HeadAsync(string requestUri, CancellationToken cancellationToken = default)`

Implements the HEAD request into a simple method. HEAD request is useful for getting the last modified date.

## Benchmarks

The library tries to be as effective as possible to an absurd level with using managed code only.

Benchmarks coming soon!