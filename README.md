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

## Text formatting

Currently only deformat.

Credits to [reaby](https://github.com/reaby) for the Regex pattern.

`Formatter.Deformat()`

```cs
var formatted = "$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01";
var deformatted = Formatter.Deformat(formatted);
Console.WriteLine(deformatted);

// Output: TMU.KrazyColors v0.1
```

## .NET Standard 2+

`HttpClientExtensions.HeadAsync()`

Implements the HEAD request into a simple method. HEAD request is useful for getting the last modified date.