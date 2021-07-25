# Trackmania Essentials

[![Nuget](https://img.shields.io/nuget/v/TmEssentials?style=for-the-badge)](https://www.nuget.org/packages/TmEssentials/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/tm-essentials?include_prereleases&style=for-the-badge)](https://github.com/BigBang1112/tm-essentials/releases)

A super light-weight library that provides formatting features.

## Time formatting

`TimeSpanExtensions.ToStringTm()`

```cs
TimeSpan time = TimeSpan.FromSeconds(23.51);
string formattedTime = time.ToStringTm();
Console.WriteLine(formattedTime);

// Output: 0:23.510
```

```cs
TimeSpan time = TimeSpan.FromSeconds(23.51);
string formattedTime = time.ToStringTm(useHundredths: true);
Console.WriteLine(formattedTime);

// Output: 0:23.51
```

```cs
TimeSpan? noTime = null;
string formattedTime = noTime.ToStringTm();
Console.WriteLine(formattedTime);

// Output: -:--.---
```

```cs
TimeSpan? noTime = null;
string formattedTime = noTime.ToStringTm(useHundredths: true);
Console.WriteLine(formattedTime);

// Output: -:--.--
```

## Text formatting

Currently only deformat.

`Formatter.Deformat()`

```cs
var formatted = "$L[goo.gl/XFp5HJ]$i$09fSYN$000.$fffDoc_Me4ik";
var deformatted = Formatter.Deformat(formatted);
Console.WriteLine(deformatted);

// Output: SYN.Doc_Me4ik
```

## .NET Standard 2+

`HttpClientExtensions.HeadAsync()`

Implements the HEAD request into a simple method. HEAD request is useful for getting the last modified date.