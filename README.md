# Trackmania Essentials

[![Nuget](https://img.shields.io/nuget/v/TmEssentials?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/TmEssentials/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/tm-essentials?include_prereleases&style=for-the-badge&logo=github)](https://github.com/BigBang1112/tm-essentials/releases)
[![Code Coverage](https://img.shields.io/badge/Code%20Coverage-96%25-success?style=for-the-badge)](https://github.com/BigBang1112/tm-essentials)

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

## Deformat

Credits to [Tom](https://github.com/ThaumicTom) and [Stefan Baumann](https://github.com/stefan-baumann) for their effective Regex pattern.

`TextFormatter.Deformat()`

```cs
var formatted = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";
var deformatted = TextFormatter.Deformat(formatted);
Console.WriteLine(deformatted);

// Output: TMU.KrazyColors v0.1
```

## HTML formatting

`TextFormatter.FormatHtml()`

```cs
var html = TextFormatter.FormatHtml("$f00Red $l[https://trackmania.io]link$l", allowLinks: true);
// Output: <span style="color:#f00">Red </span><a href="https://trackmania.io" style="color:#f00">link</a>
```

Links are safe to show from untrusted text. Only `http://` and `https://` URLs become links, URLs without a scheme (like `www.trackmania.com`) get `http://` in front, and URLs with any other scheme (like `javascript:`) are shown as plain text.

### Icons

Trackmania renders the private use characters U+F000 to U+F2E0 as [Font Awesome 4.7](https://fontawesome.com/v4/icons/) icons (see [Text Styling](https://wiki.trackmania.io/en/content-creation/text-styling#font-awesome)). Pass a CSS class to wrap them in `<span>` elements, so that a page can display them with the icon font:

```cs
var html = TextFormatter.FormatHtml(" Winner", allowLinks: false, allowManialinks: false, iconCssClass: "fa");
// Output: <span class="fa"></span> Winner
```

`fa` works with the Font Awesome 4.7 stylesheet, or use `tm-icon` with the stylesheet of TmEssentials.Razor.

## Blazor

The TmEssentials.Razor package provides the `FormattedText` component:

```razor
<FormattedText Text="$f00Red $l[https://trackmania.io]link$l" AllowLinks="true" />
```

To display the icons, add the stylesheet that comes with the package (it includes the Font Awesome 4.7 font, licensed under the SIL OFL 1.1):

```html
<link rel="stylesheet" href="_content/TmEssentials.Razor/tm-essentials.css" />
```

Set `IconCssClass="fa"` if the page uses the Font Awesome 4.7 stylesheet instead, or `IconCssClass="@null"` to not wrap the icons.

## ANSI formatting

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

Map UIDs are generated the same way as in the game: a new UUID and a CRC32 (20 bytes) written as one number in base 63 with `_0-9a-zA-Z` digits. They are most often 27 characters long, sometimes shorter.

```cs
MapUtils.GenerateMapUid() // random, like "az8D4K6OKOuyGtUjHV_ui39x4xi"
MapUtils.GenerateMapUid(seed: 1112) // the same UID for the same seed
MapUtils.GenerateMapUid(uuid, crc32) // exactly what the game would make from these

MapUtils.TryDecodeMapUid("az8D4K6OKOuyGtUjHV_ui39x4xi", out var uuid, out var crc32)
// eabd691c-a87f-428c-9e56-608e21bc91dd, 0xCF9B930D
```

Since the game uses the CRC32 of the map's serialized data, which needs the whole map, the random overloads use a random number in its place.

## .NET Standard 2+

`HttpClientExtensions.HeadAsync(string requestUri, CancellationToken cancellationToken = default)`

Implements the HEAD request into a simple method. HEAD request is useful for getting the last modified date.

## Benchmarks

The library tries to be as effective as possible to an absurd level with using managed code only.

```
BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.9974)
AMD Ryzen 9 9900X 4.39GHz, 1 CPU, 24 logical and 12 physical cores
Memory: 31,14 GB Total, 14,11 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4
  DefaultJob : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v4


| Method                                                   | Mean       | Error     | StdDev    | Gen0   | Allocated |
|--------------------------------------------------------- |-----------:|----------:|----------:|-------:|----------:|
| 'Null with hundredths'                                   |  0.9312 ns | 0.0029 ns | 0.0024 ns |      - |         - |
| 'Null with milliseconds'                                 |  0.9352 ns | 0.0069 ns | 0.0058 ns |      - |         - |
| '1-digit minute, seconds and milliseconds only'          |  9.4585 ns | 0.0532 ns | 0.0472 ns | 0.0024 |      40 B |
| '2-digit minute, seconds and hundredths only'            |  9.5386 ns | 0.0673 ns | 0.0630 ns | 0.0024 |      40 B |
| 'Negative 2-digit minute, seconds and hundredths only'   |  9.6299 ns | 0.0321 ns | 0.0301 ns | 0.0024 |      40 B |
| 'Negative seconds and hundredths only'                   |  9.6422 ns | 0.0610 ns | 0.0571 ns | 0.0024 |      40 B |
| 'Negative 1-digit minute, seconds and hundredths only'   |  9.7765 ns | 0.0748 ns | 0.0700 ns | 0.0024 |      40 B |
| '1-digit minute, seconds and hundredths only'            |  9.7839 ns | 0.0973 ns | 0.0911 ns | 0.0024 |      40 B |
| 'Negative 1-digit minute, seconds and milliseconds only' |  9.7875 ns | 0.0229 ns | 0.0191 ns | 0.0024 |      40 B |
| 'Seconds and hundredths only'                            |  9.8086 ns | 0.0661 ns | 0.0618 ns | 0.0024 |      40 B |
| 'Seconds and milliseconds only'                          |  9.8104 ns | 0.0679 ns | 0.0635 ns | 0.0024 |      40 B |
| 'Negative hundredths only'                               |  9.8637 ns | 0.0628 ns | 0.0587 ns | 0.0024 |      40 B |
| 'Negative milliseconds only'                             |  9.8754 ns | 0.0784 ns | 0.0734 ns | 0.0024 |      40 B |
| 'Negative seconds and milliseconds only'                 |  9.9648 ns | 0.0209 ns | 0.0163 ns | 0.0024 |      40 B |
| 'Hundredths only'                                        | 10.0018 ns | 0.1943 ns | 0.1908 ns | 0.0024 |      40 B |
| 'Milliseconds only'                                      | 10.0217 ns | 0.1773 ns | 0.1658 ns | 0.0024 |      40 B |
| '2-digit minute, seconds and milliseconds only'          | 10.0368 ns | 0.0647 ns | 0.0605 ns | 0.0024 |      40 B |
| 'Negative 2-digit minute, seconds and milliseconds only' | 10.1620 ns | 0.0783 ns | 0.0732 ns | 0.0029 |      48 B |
| 'Hours, minutes, seconds and hundredths only'            | 11.0210 ns | 0.0900 ns | 0.0842 ns | 0.0029 |      48 B |
| 'Hours, minutes, seconds and milliseconds only'          | 11.0689 ns | 0.0841 ns | 0.0787 ns | 0.0029 |      48 B |
| 'Negative hours, minutes, seconds and milliseconds only' | 11.2956 ns | 0.0226 ns | 0.0176 ns | 0.0029 |      48 B |
| 'Negative hours, minutes, seconds and hundredths only'   | 11.6347 ns | 0.0687 ns | 0.0574 ns | 0.0029 |      48 B |
| 'Fully-used TimeSpan with hundredths'                    | 11.8304 ns | 0.0577 ns | 0.0540 ns | 0.0029 |      48 B |
| 'Fully-used negative TimeSpan with hundredths'           | 12.2413 ns | 0.0195 ns | 0.0162 ns | 0.0033 |      56 B |
| 'Fully-used TimeSpan with milliseconds'                  | 12.4213 ns | 0.0826 ns | 0.0773 ns | 0.0033 |      56 B |
| 'Fully-used negative TimeSpan with milliseconds'         | 12.6353 ns | 0.0227 ns | 0.0190 ns | 0.0033 |      56 B |
```
