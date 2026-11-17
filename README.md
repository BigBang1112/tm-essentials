![TmEssentials](https://raw.githubusercontent.com/BigBang1112/tm-essentials/main/logo_icon_outline.png)

[![NuGet](https://img.shields.io/nuget/v/TmEssentials?style=for-the-badge&logo=nuget&label=TmEssentials)](https://www.nuget.org/packages/TmEssentials/)
[![NuGet](https://img.shields.io/nuget/v/TmEssentials.Razor?style=for-the-badge&logo=nuget&label=TmEssentials.Razor)](https://www.nuget.org/packages/TmEssentials.Razor/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/tm-essentials?include_prereleases&style=for-the-badge&logo=github)](https://github.com/BigBang1112/tm-essentials/releases)
[![Code Coverage](https://img.shields.io/badge/Code%20Coverage-100%25-success?style=for-the-badge)](https://github.com/BigBang1112/tm-essentials)

Welcome to TmEssentials!

A light-weight .NET library with the essentials of Trackmania and ManiaPlanet:

- **Race times** with `TimeInt32` and `TimeSingle`, formatted and parsed like in the game
- **Text formatting** that reads `$` codes **the same way as the game**, to plain text, HTML or ANSI
- **Account IDs and logins** converted both ways
- **Map UIDs** made and decoded like the game does
- Fully managed, **trimmable** and **NativeAOT compatible**
- Runs anywhere from **.NET Framework 4.6.2** to **.NET 11**

Try the text formatting in the [web sample](https://bigbang1112.github.io/tm-essentials/).

## Packages

Package | Description
--- | ---
[TmEssentials](https://www.nuget.org/packages/TmEssentials/) | Times, text formatting, accounts and map UIDs
[TmEssentials.Razor](https://www.nuget.org/packages/TmEssentials.Razor/) | The `FormattedText` Blazor component, with a stylesheet for the game's icons

```
dotnet add package TmEssentials
```

## Framework support

TmEssentials targets .NET 11, 10, 9 and 8, .NET Standard 2.1 and 2.0, and .NET Framework 4.6.2. TmEssentials.Razor targets .NET 11, 10, 9 and 8.

Feature | .NET 8+ | .NET Standard 2.0/2.1 | .NET Framework 4.6.2+
--- | --- | --- | ---
Times, text formatting, accounts, map UIDs | Yes | Yes | Yes
`IParsable` and `ISpanParsable` on times | Yes | No | No
System.Text.Json converters | Yes | No | No
`HttpClient.HeadAsync` | Yes | Yes | No

## Time

### TimeInt32 and TimeSingle

The game stores race times as whole milliseconds in an `int`, and other times, like MediaTracker keys, as seconds in a `float`. `TimeInt32` and `TimeSingle` hold exactly that in 4 bytes, half the size of `TimeSpan`. They have the familiar `TimeSpan` members (`Days`, `TotalSeconds`, `Add`, `Duration`, operators and more), share the `ITime` interface, and `ToString()` writes the time in the Trackmania format.

```cs
var time = new TimeInt32(TotalMilliseconds: 36217);

time.ToTmString();                    // 0:36.217 (ToString() does the same)
time.ToTmString(useHundredths: true); // 0:36.21
time.ToTmString(useApostrophe: true); // 0'36''217
time.ToTmString(compact: true);       // 36.217, handy for time differences

new TimeInt32(3_723_456).ToString();  // 1:02:03.456

TimeInt32? noTime = null;
noTime.ToTmString();                    // -:--.---
noTime.ToTmString(useHundredths: true); // -:--.--
noTime.ToTmString(useApostrophe: true); // -'--''---
```

`TimeSingle` works the same way. It rounds to the nearest millisecond when formatting, so float errors don't show:

```cs
var time = new TimeSingle(TotalSeconds: 23.51f);

time.ToTmString();                    // 0:23.510
time.ToTmString(useHundredths: true); // 0:23.51

new TimeSingle(0.251f).ToString();    // 0:00.251, even though the float is 0.25099998
```

### Parsing

Both types parse the formats that `ToString()` writes, with milliseconds or hundredths and an optional minus sign. Parsing does not depend on the culture.

```cs
TimeInt32.Parse("1:23.456");                 // 83456 milliseconds
TimeInt32.Parse("36.217");                   // 36217 milliseconds
TimeInt32.TryParse("abc", out var result);   // false

TimeSingle.Parse("1:23.456");                // 83.456 seconds
```

> [!NOTE]
> A number without a colon or a dot is read as a fraction of a second, like `TimeSpan.ParseExact` with the `FFFFFFF` format. `5` is 500 milliseconds, `05` is 50 milliseconds and `500` is 500 milliseconds.

### Conversions and overflow

`TimeInt32` behaves like `TimeSpan` when a value doesn't fit: it throws `OverflowException` instead of wrapping around. Its range is about ±24.8 days.

```cs
TimeSpan timeSpan = new TimeInt32(1234);       // implicit
TimeSingle single = new TimeInt32(1234);       // implicit

var a = (TimeInt32)TimeSpan.FromSeconds(1.2345); // 1234 ms, truncated to whole milliseconds
var b = (TimeInt32)new TimeSingle(0.251f);       // 251 ms, rounded to the nearest millisecond
var c = TimeInt32.FromSeconds(0.251f);           // 251 ms, rounded to the nearest millisecond

new TimeInt32(251) == new TimeSingle(0.251f);    // true, compared by TotalSeconds

TimeInt32.MaxValue + new TimeInt32(1);           // OverflowException
TimeInt32.FromDays(30);                          // OverflowException
```

> [!NOTE]
> These are breaking changes from 2.8.0: conversions to `TimeInt32` from `TimeSpan` and `TimeSingle` need a cast, the `From` methods round instead of truncating, and arithmetic throws on overflow. See the [releases](https://github.com/BigBang1112/tm-essentials/releases) for the full list.

### TimeSpan extensions

The same formatting is available on `TimeSpan`:

```cs
TimeSpan.FromMilliseconds(83456).ToTmString(); // 1:23.456

TimeSpan? noTime = null;
noTime.ToTmString();                           // -:--.---

TimeSpan.FromTicks(15_000).ToMilliseconds();   // 1, the total milliseconds truncated to an int
```

### JSON and TypeConverter

On .NET 8+, `JsonTimeInt32Converter` and `JsonTimeSingleConverter` read and write the times as JSON numbers: `TimeInt32` as milliseconds and `TimeSingle` as seconds. They use no reflection, so they also work in source-generated contexts.

```cs
var options = new JsonSerializerOptions
{
    Converters = { new JsonTimeInt32Converter(), new JsonTimeSingleConverter() }
};

JsonSerializer.Serialize(new { Time = new TimeInt32(36217), Key = new TimeSingle(1.5f) }, options);
// {"Time":36217,"Key":1.5}
```

Both types also have a `TypeConverter`, so configuration and input binding can read times from strings like `1:23.456`.

## Text formatting

`TextFormatter` reads formatting codes the same way as the formatting parser of ManiaPlanet:

- Colors with 1 to 3 hex digits (`$f` is `$f00`)
- `$<` and `$>` save and restore up to 7 styles, and `$z` and `$g` reset to the style saved by `$<`
- `$l`, `$h` and `$p` links, with or without `[url]`
- The `$$`, `$[` and `$]` escapes
- Carriage returns and a trailing `$` are removed, like in the game

### Deformat

`TextFormatter.Deformat()` removes the formatting codes:

```cs
TextFormatter.Deformat("$F00T$D01M$C13U$A14.$815K$727r$528a$329z$23By$03CC$03Co$04Bl$059o$068r$077s$085 $094v$0A30$0B1.$0C01");
// TMU.KrazyColors v0.1

TextFormatter.Deformat("$o$f00Big $<$iRed$> $$5 $l[https://trackmania.io]link$l");
// Big Red $5 link
```

### HTML

`TextFormatter.FormatHtml()` turns the colors and styles into inline CSS, and the links into `<a>` elements when they are allowed:

```cs
TextFormatter.FormatHtml("$f00Red $l[https://trackmania.io]link$l", allowLinks: true);
// <span style="color:#f00">Red </span><a href="https://trackmania.io" style="color:#f00">link</a>

TextFormatter.FormatHtml("$o$iBold italic$z $sShadow");
// <span style="font-weight:bold;font-style:italic">Bold italic</span> <span style="text-shadow:1px 1px 2px black">Shadow</span>

TextFormatter.FormatHtml("$h[maniaplanet]Home$h", allowManialinks: true);
// <a href="maniaplanet:///:maniaplanet">Home</a>
```

> [!NOTE]
> Links are safe to show from untrusted text. Only `http://` and `https://` URLs become links, URLs without a scheme (like `www.trackmania.com`) get `http://` in front, and URLs with any other scheme (like `javascript:`) are shown as plain text.

### Icons

The game renders the private use characters U+F000 to U+F2E0 as [Font Awesome 4.7](https://fontawesome.com/v4/icons/) icons (see [Text Styling](https://wiki.trackmania.io/en/content-creation/text-styling#font-awesome)). Pass a CSS class to wrap them in `<span>` elements, so that a page can display them with the icon font:

```cs
TextFormatter.FormatHtml(" Winner", allowLinks: false, allowManialinks: false, iconCssClass: "fa");
// <span class="fa"></span> Winner
```

`fa` works with the Font Awesome 4.7 stylesheet, or use `tm-icon` with the stylesheet of TmEssentials.Razor. `TextFormatter.IsFontAwesomeIcon(char)` tells whether a character is one of these icons.

### ANSI

`TextFormatter.FormatAnsi()` turns the colors into the closest ANSI colors for terminals:

```cs
TextFormatter.FormatAnsi("$f00Red $0f0Green $zDefault");
// \x1B[1;31mRed \x1B[1;32mGreen \x1B[39m\x1B[22mDefault\x1B[39m\x1B[22m
```

### Regex pattern

`TextFormatter.FormattingCodeRegexPattern` matches one formatting code, for when you need the codes themselves:

```cs
Regex.Matches("$f00Red $l[https://trackmania.io]link$l", TextFormatter.FormattingCodeRegexPattern);
// $f00, $l[https://trackmania.io], $l
```

> [!NOTE]
> A regex cannot know whether a link is open, so inside a link it can read more than the formatters do. Prefer the `TextFormatter` methods when you only need the result.

## Blazor

TmEssentials.Razor provides the `FormattedText` component. Add `@using TmEssentials.Razor` to `_Imports.razor`, then:

```razor
<FormattedText Text="$f00Red $l[https://trackmania.io]link$l" AllowLinks="true" />
```

To display the icons, add the stylesheet that comes with the package (it includes the Font Awesome 4.7 font, licensed under the SIL OFL 1.1):

```html
<link rel="stylesheet" href="_content/TmEssentials.Razor/tm-essentials.css" />
```

Set `IconCssClass="fa"` if the page uses the Font Awesome 4.7 stylesheet instead, or `IconCssClass="@null"` to not wrap the icons.

## Accounts

`AccountUtils` converts between account IDs and logins:

```cs
AccountUtils.ToAccountId("v89i_w-eQKq5JBG5xwuKCQ");                     // bfcf62ff-0f9e-40aa-b924-11b9c70b8a09
AccountUtils.ToLogin(Guid.Parse("bfcf62ff-0f9e-40aa-b924-11b9c70b8a09")); // v89i_w-eQKq5JBG5xwuKCQ
```

`ToAccountId` throws `FormatException` when the login does not decode to exactly 16 bytes.

## Map UIDs

`MapUtils` makes map UIDs the same way as the game: a new UUID and a CRC32 (20 bytes) written as one number in base 63, with `_0-9a-zA-Z` as the digits. They are most often 27 characters long, sometimes shorter.

```cs
MapUtils.GenerateMapUid();             // random, like az8D4K6OKOuyGtUjHV_ui39x4xi
MapUtils.GenerateMapUid(seed: 1112);   // OQNeAcbA0Uh7ftCm_tRbW7wezdb, the same for the same seed
MapUtils.GenerateMapUid(uuid, crc32);  // exactly what the game makes from these

MapUtils.TryDecodeMapUid("az8D4K6OKOuyGtUjHV_ui39x4xi", out var uuid, out var crc32);
// true, eabd691c-a87f-428c-9e56-608e21bc91dd, 0xCF9B930D
```

The game uses the CRC32 of the map's serialized data, which needs the whole map, so the random overloads use a random number in its place.

## HTTP HEAD requests

`HttpClientExtensions.HeadAsync()` sends a HEAD request, which is useful for checking a file's size or last modified date without downloading it:

```cs
using var response = await http.HeadAsync("https://example.com/map.Map.Gbx");
var lastModified = response.Content.Headers.LastModified;
```

## Benchmarks

The benchmarks are in [Tests/TmEssentials.Benchmarks](Tests/TmEssentials.Benchmarks). Run them with:

```
dotnet run -c Release --project Tests/TmEssentials.Benchmarks/TmEssentials.Benchmarks.csproj
```

## Build

Make sure you have the [.NET 11 SDK](https://dotnet.microsoft.com/download) installed, then:

```
git clone https://github.com/BigBang1112/tm-essentials.git
cd tm-essentials
dotnet build
dotnet test --solution TmEssentials.slnx
```

> [!NOTE]
> The browser tests of TmEssentials.Razor use the installed Google Chrome, and the .NET Framework tests only run on Windows.

## License

- TmEssentials and TmEssentials.Razor are licensed under the [MIT License](LICENSE).
- The Font Awesome 4.7 font included in TmEssentials.Razor is licensed under the [SIL OFL 1.1](Src/TmEssentials.Razor/wwwroot/fonts/FontAwesome-OFL.txt).
