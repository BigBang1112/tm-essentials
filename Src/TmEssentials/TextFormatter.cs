using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using System.Text;

namespace TmEssentials;

/// <summary>
/// Provides a set of methods for text formatting in Nadeo games.
/// </summary>
/// <remarks>
/// The parsing rules follow the formatting parser of ManiaPlanet (<c>CFastStringFormatting::GetMarkup</c> and <c>ReadNextChar</c>).
/// </remarks>
public static class TextFormatter
{
    //
    // Credits to reaby for the ANSI stuff (https://github.com/reaby)
    //

    internal const string AnsiDefault = "\x1B[39m\x1B[22m";

    /// <summary>
    /// A regex pattern that matches one formatting code as the formatters read it when no link is open: a dollar sign followed by
    /// a link code with its <c>[url]</c> (only the <c>[</c> when the bracket is not closed, so the rest stays visible),
    /// a color of 1 to 3 hex digits, any other character (a surrogate pair counts as one), or nothing (at the end of the string).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The pattern does not depend on regex options. Line breaks are matched like any other character, as in the game.
    /// </para>
    /// <para>
    /// A regex cannot know whether a link is open. Inside a link, the formatters read any link code as just the two characters that
    /// close the link, so in <c>$l[x]a$l[y]b</c> the formatters show <c>[y]</c> as text, while the pattern matches <c>$l[y]</c>.
    /// The pattern also matches the escapes <c>$$</c>, <c>$[</c> and <c>$]</c> like other codes, while the formatters show them as one character.
    /// </para>
    /// </remarks>
#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Regex)]
#endif
    public const string FormattingCodeRegexPattern = @"\$(?:[hlpHLP](?:\[[^\]]*\]|\[)?|[0-9a-fA-F]{1,3}|[\uD800-\uDBFF][\s\S]?|[\s\S])?";

    /// <summary>
    /// The game keeps the base style plus up to 7 styles saved by <c>$&lt;</c>. Further <c>$&lt;</c> are ignored.
    /// </summary>
    private const int MaxStyleStackCount = 7;

    /// <summary>
    /// The characters that the game trims around a link URL before opening it (<c>CFastString::s_DefaultWhiteSpaceChars</c>).
    /// </summary>
    private static readonly char[] UrlWhiteSpaceChars = [' ', '\t', '\r', '\n'];

    /// <summary>
    /// The characters that are not copied as they are: the start of a formatting code, and the carriage return that is not displayed.
    /// Text without them is the same after formatting.
    /// </summary>
    private static readonly char[] SpecialChars = ['$', '\r'];

    /// <summary>
    /// The ANSI color codes of the 4096 colors (<c>$000</c> to <c>$fff</c>), computed when first used, or 0 when not computed yet.
    /// 30 to 37 are the normal colors, 90 to 97 the bright ones.
    /// </summary>
    private static readonly byte[] AnsiColorCodes = new byte[4096];

    /// <summary>
    /// The largest difference between the hex digits of a color (like 1 in <c>$988</c>) for which the ANSI formatter uses a gray instead of a color.
    /// </summary>
    private const int AnsiGrayMaxChannelSpread = 1;

    /// <summary>
    /// Deformats a string from Nadeo format.
    /// </summary>
    /// <param name="input">A string input.</param>
    /// <returns>A deformatted string.</returns>
    /// <exception cref="ArgumentNullException">Input is null.</exception>
    public static string Deformat(string input)
    {
        return Deformat(input, -1);
    }

    /// <summary>
    /// Deformats a string from Trackmania/Shootmania format.
    /// </summary>
    /// <param name="input">A string input.</param>
    /// <param name="maxReplacementCount">The maximum number of formatting codes to remove. The rest of the string is kept as is. A negative value removes all of them.</param>
    /// <returns>A deformatted string.</returns>
    /// <exception cref="ArgumentNullException">Input is null.</exception>
    public static string Deformat(string input, int maxReplacementCount)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (input.IndexOfAny(SpecialChars) == -1)
        {
            return input;
        }

        var output = new StringBuilder(input.Length);
        var inLink = false;
        var replacementCount = 0;

        // The link states saved by $<, the last saved one in the lowest bit
        var savedLinkStates = 0;
        var savedCount = 0;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (c != '$')
            {
                if (c != '\r')
                {
                    i = AppendText(output, input, i, uppercase: false) - 1;
                }

                continue;
            }

            if (maxReplacementCount >= 0 && replacementCount >= maxReplacementCount)
            {
                output.Append(input, i, input.Length - i);
                break;
            }

            replacementCount++;

            var markup = ReadMarkup(input, i, inLink);
            i += markup.Length - 1;

            switch (markup.Type)
            {
                case MarkupType.Literal:
                    output.Append(markup.Literal);
                    break;
                case MarkupType.Push:
                    if (savedCount < MaxStyleStackCount)
                    {
                        savedLinkStates = (savedLinkStates << 1) | (inLink ? 1 : 0);
                        savedCount++;
                    }
                    break;
                case MarkupType.Pop:
                    if (savedCount > 0)
                    {
                        inLink = (savedLinkStates & 1) != 0;
                        savedLinkStates >>= 1;
                        savedCount--;
                    }
                    break;
                case MarkupType.Reset:
                    inLink = savedCount > 0 && (savedLinkStates & 1) != 0;
                    break;
                case MarkupType.LinkStart:
                    inLink = true;
                    break;
                case MarkupType.LinkEnd:
                    inLink = false;
                    break;
            }
        }

        return output.ToString();
    }

    /// <summary>
    /// Formats a string to ANSI format.
    /// </summary>
    /// <param name="input">A string input.</param>
    /// <returns>A string with colors as ANSI escape codes.</returns>
    /// <exception cref="ArgumentNullException">Input is null.</exception>
    public static string FormatAnsi(string input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (input.IndexOfAny(SpecialChars) == -1)
        {
            return input + AnsiDefault;
        }

        var output = new StringBuilder(input.Length + AnsiDefault.Length);
        var state = default(AnsiState);
        Stack<AnsiState>? savedStates = null;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (c != '$')
            {
                if (c != '\r')
                {
                    i = AppendText(output, input, i, state.Uppercase) - 1;
                }

                continue;
            }

            var markup = ReadMarkup(input, i, state.InLink);
            var markupIndex = i;
            i += markup.Length - 1;

            switch (markup.Type)
            {
                case MarkupType.Literal:
                    output.Append(markup.Literal);
                    break;
                case MarkupType.Color:
                    state.Color = ReadColor(input, markupIndex, markup.Length);
                    AppendAnsiColor(output, state.Color);
                    break;
                case MarkupType.DefaultColor:
                    state.Color = savedStates is { Count: > 0 } ? savedStates.Peek().Color : AnsiState.NoColor;
                    AppendAnsiColor(output, state.Color);
                    break;
                case MarkupType.Reset:
                    state = savedStates is { Count: > 0 } ? savedStates.Peek() : default;
                    AppendAnsiColor(output, state.Color);
                    break;
                case MarkupType.Push:
                    savedStates ??= new Stack<AnsiState>(MaxStyleStackCount);

                    if (savedStates.Count < MaxStyleStackCount)
                    {
                        savedStates.Push(state);
                    }
                    break;
                case MarkupType.Pop:
                    if (savedStates is { Count: > 0 })
                    {
                        state = savedStates.Pop();
                        AppendAnsiColor(output, state.Color);
                    }
                    break;
                case MarkupType.Uppercase:
                    state.Uppercase = !state.Uppercase;
                    break;
                case MarkupType.LinkStart:
                    state.InLink = true;
                    break;
                case MarkupType.LinkEnd:
                    state.InLink = false;
                    break;
            }
        }

        output.Append(AnsiDefault);

        return output.ToString();
    }

    /// <summary>
    /// Appends the ANSI escape codes of a 12-bit color like <c>0xF00</c>, or of the default color if it is <see cref="AnsiState.NoColor"/>.
    /// </summary>
    private static void AppendAnsiColor(StringBuilder output, int color)
    {
        if (color == AnsiState.NoColor)
        {
            output.Append(AnsiDefault);
            return;
        }

        var code = AnsiColorCodes[color];

        // Computing the code twice in parallel is fine, the result is the same
        if (code == 0)
        {
            code = GetAnsiColorCode(color);
            AnsiColorCodes[color] = code;
        }

        output.Append('\x1B')
            .Append('[')
            .Append((char)('0' + code / 10))
            .Append((char)('0' + code % 10))
            .Append('m');
    }

    /// <summary>
    /// Finds the ANSI color closest to a 12-bit color like <c>0xF00</c>.
    /// </summary>
    /// <returns>The ANSI color code: 30 to 37 for a normal color, 90 to 97 for a bright one.</returns>
    private static byte GetAnsiColorCode(int color)
    {
        var r4 = (color >> 8) & 0xF;
        var g4 = (color >> 4) & 0xF;
        var b4 = color & 0xF;

        var max = Math.Max(r4, Math.Max(g4, b4));
        var min = Math.Min(r4, Math.Min(g4, b4));

        // Grays and colors close to gray have no clear hue, so they get a gray by their lightness instead
        if (max - min <= AnsiGrayMaxChannelSpread)
        {
            return GetAnsiGrayCode(max + min);
        }

        var argb = Color.FromArgb(0x11 * r4, 0x11 * g4, 0x11 * b4);
        var lightness = argb.GetBrightness();

        // Very dark colors are dark gray (bright black), so that they stay visible on dark terminals
        if (lightness < 0.1)
        {
            return 90;
        }

        // Very pale colors are white (bright white)
        if (lightness > 0.9)
        {
            return 97;
        }

        byte code = argb.GetHue() switch
        {
            < 30 => 31,// red
            < 80 => 33,// yellow
            < 160 => 32,// green
            < 214 => 36,// cyan
            < 284 => 34,// blue
            < 333 => 35,// magenta
            _ => 31,
        };

        // The bright colors are 60 above the normal ones
        return lightness > 0.43 ? (byte)(code + 60) : code;
    }

    /// <summary>
    /// Finds the ANSI gray closest to a gray color.
    /// </summary>
    /// <param name="lightness">The sum of the highest and lowest hex digit of the color (0 to 30).</param>
    /// <returns>The ANSI color code: 90 (dark gray), 37 (light gray) or 97 (white).</returns>
    private static byte GetAnsiGrayCode(int lightness)
    {
        return lightness switch
        {
            // $000 to $aaa: dark gray (bright black), so that black stays visible on dark terminals
            <= 20 => 90,
            // $bbb to $ddd: light gray (white)
            <= 27 => 37,
            // $eee and $fff: white (bright white)
            _ => 97,
        };
    }

    internal static void AppendAnsiText(StringBuilder output, string element)
    {
        if (element.Length == 0)
        {
            return;
        }

        if (element[0] != '$')
        {
            output.Append(element);
            return;
        }

        if (element.Length == 2 && element[1] is 'z' or 'Z' or 'g' or 'G')
        {
            output.Append(AnsiDefault);
            return;
        }

        if (element.Length is < 2 or > 4)
        {
            return;
        }

        for (var i = 1; i < element.Length; i++)
        {
            if (!IsHex(element[i]))
            {
                return;
            }
        }

        AppendAnsiColor(output, ReadColor(element, 0, element.Length));
    }

    /// <summary>
    /// Checks if the character is a Font Awesome 4.7 icon. The game renders these private use characters (U+F000 to U+F2E0) as icons.
    /// </summary>
    /// <param name="c">A character.</param>
    /// <returns>True if the character is a Font Awesome 4.7 icon, otherwise false.</returns>
    public static bool IsFontAwesomeIcon(char c) => c is >= '' and <= '';

    /// <summary>
    /// Formats a Nadeo string into a HTML format.
    /// </summary>
    /// <param name="input">The input Nadeo string.</param>
    /// <param name="allowLinks">Whether standard links ($l) should be processed.</param>
    /// <param name="allowManialinks">Whether manialinks ($h and $p) should be processed.</param>
    /// <returns>A Nadeo string formatted as HTML.</returns>
    public static string FormatHtml(string? input, bool allowLinks = false, bool allowManialinks = false)
    {
        return FormatHtml(input, allowLinks, allowManialinks, iconCssClass: null);
    }

    /// <summary>
    /// Formats a Nadeo string into a HTML format, with the Font Awesome icons wrapped in elements of a CSS class.
    /// </summary>
    /// <param name="input">The input Nadeo string.</param>
    /// <param name="allowLinks">Whether standard links ($l) should be processed.</param>
    /// <param name="allowManialinks">Whether manialinks ($h and $p) should be processed.</param>
    /// <param name="iconCssClass">
    /// The CSS class of the <c>span</c> elements wrapped around Font Awesome icons (see <see cref="IsFontAwesomeIcon(char)"/>),
    /// so that they can be displayed with a Font Awesome 4.7 font. For example, <c>fa</c> with the Font Awesome 4.7 stylesheet.
    /// Null to not wrap the icons.
    /// </param>
    /// <returns>A Nadeo string formatted as HTML.</returns>
    public static string FormatHtml(string? input, bool allowLinks, bool allowManialinks, string? iconCssClass)
    {
        if (input is null or "")
        {
            return string.Empty;
        }

        var iconStartTag = string.IsNullOrEmpty(iconCssClass) ? null : $"<span class=\"{WebUtility.HtmlEncode(iconCssClass)}\">";

        // Text without formatting only needs to be encoded
        if (input.IndexOfAny(SpecialChars) == -1)
        {
            if (iconStartTag is null)
            {
                return WebUtility.HtmlEncode(input);
            }

            var plainOutput = new StringBuilder();
            AppendHtmlText(plainOutput, input, iconStartTag);
            return plainOutput.ToString();
        }

        var output = new StringBuilder(input.Length * 2);
        var text = new StringBuilder();
        var style = default(HtmlStyle);
        Stack<HtmlStyle>? savedStyles = null;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (c != '$')
            {
                if (c != '\r')
                {
                    i = AppendText(text, input, i, style.Uppercase) - 1;
                }

                continue;
            }

            var markup = ReadMarkup(input, i, style.InLink);
            var markupIndex = i;
            i += markup.Length - 1;

            if (markup.Type == MarkupType.Literal)
            {
                text.Append(markup.Literal);
                continue;
            }

            if (markup.Type is MarkupType.Ignored or MarkupType.Underline)
            {
                continue;
            }

            // The text so far has the style before this code
            AppendHtmlSegment(output, text, style, input, iconStartTag);

            switch (markup.Type)
            {
                case MarkupType.Color:
                    style.ColorIndex = markupIndex + 1;
                    style.ColorLength = markup.Length - 1;
                    break;
                case MarkupType.DefaultColor:
                    var savedStyle = savedStyles is { Count: > 0 } ? savedStyles.Peek() : default;
                    style.ColorIndex = savedStyle.ColorIndex;
                    style.ColorLength = savedStyle.ColorLength;
                    break;
                case MarkupType.Reset:
                    style = savedStyles is { Count: > 0 } ? savedStyles.Peek() : default;
                    break;
                case MarkupType.Push:
                    savedStyles ??= new Stack<HtmlStyle>(MaxStyleStackCount);

                    if (savedStyles.Count < MaxStyleStackCount)
                    {
                        savedStyles.Push(style);
                    }
                    break;
                case MarkupType.Pop:
                    if (savedStyles is { Count: > 0 })
                    {
                        style = savedStyles.Pop();
                    }
                    break;
                case MarkupType.Italic: style.Italic = !style.Italic; break;
                case MarkupType.Bold: style.Bold = !style.Bold; break;
                case MarkupType.Shadow: style.DropShadow = !style.DropShadow; break;
                case MarkupType.Uppercase: style.Uppercase = !style.Uppercase; break;
                case MarkupType.Wide: style.Width = TextWidth.Wide; break;
                case MarkupType.Medium: style.Width = TextWidth.Medium; break;
                case MarkupType.Narrow: style.Width = TextWidth.Narrow; break;
                case MarkupType.LinkStart:
                    style.InLink = true;
                    style.LinkUrl = markup.LinkType == LinkType.External
                        ? (allowLinks ? ToSafeExternalUrl(GetLinkUrl(input, markup)) : null)
                        : (allowManialinks ? "maniaplanet:///:" + GetLinkUrl(input, markup) : null);
                    break;
                default:
                    // MarkupType.LinkEnd is the only one left, the markups without style were skipped above
                    style.InLink = false;
                    style.LinkUrl = null;
                    break;
            }
        }

        AppendHtmlSegment(output, text, style, input, iconStartTag);

        return output.ToString();
    }

    /// <summary>
    /// Appends the text with its style as HTML, and clears the text.
    /// </summary>
    private static void AppendHtmlSegment(StringBuilder output, StringBuilder text, in HtmlStyle style, string input, string? iconStartTag)
    {
        if (text.Length == 0)
        {
            return;
        }

        var segmentText = text.ToString();
        text.Clear();

        if (!string.IsNullOrEmpty(style.LinkUrl))
        {
            output.Append("<a href=\"").Append(WebUtility.HtmlEncode(style.LinkUrl)).Append('"');

            if (style.HasCss)
            {
                output.Append(" style=\"");
                AppendCss(output, style, input);
                output.Append('"');
            }

            output.Append('>');
            AppendHtmlText(output, segmentText, iconStartTag);
            output.Append("</a>");
        }
        else if (!style.HasCss)
        {
            AppendHtmlText(output, segmentText, iconStartTag);
        }
        else
        {
            output.Append("<span style=\"");
            AppendCss(output, style, input);
            output.Append("\">");
            AppendHtmlText(output, segmentText, iconStartTag);
            output.Append("</span>");
        }
    }

    private static void AppendCss(StringBuilder output, in HtmlStyle style, string input)
    {
        var start = output.Length;

        // Missing color channels are zero, like in the game ($f is $f00)
        if (style.ColorLength > 0)
        {
            output.Append("color:#")
                .Append(input, style.ColorIndex, style.ColorLength)
                .Append('0', 3 - style.ColorLength);
        }

        if (style.Bold) AppendCssDeclaration(output, start, "font-weight:bold");
        if (style.Italic) AppendCssDeclaration(output, start, "font-style:italic");
        if (style.Uppercase) AppendCssDeclaration(output, start, "text-transform:uppercase");
        if (style.DropShadow) AppendCssDeclaration(output, start, "text-shadow:1px 1px 2px black");
        if (style.Width == TextWidth.Narrow) AppendCssDeclaration(output, start, "letter-spacing:-0.05em");
    }

    private static void AppendCssDeclaration(StringBuilder output, int cssStart, string declaration)
    {
        if (output.Length > cssStart)
        {
            output.Append(';');
        }

        output.Append(declaration);
    }

    /// <summary>
    /// Makes the URL of a <c>$l</c> link safe to use as <c>href</c>, so that formatted text from players cannot run scripts
    /// (<c>javascript:</c>, <c>data:</c>, ...). HTTP(S) URLs are kept, URLs without a scheme get <c>http://</c>
    /// (like <c>www.example.com</c>), and URLs with any other scheme are not linked.
    /// </summary>
    /// <returns>A URL that starts with <c>http://</c> or <c>https://</c>, or null if the link should not be clickable.</returns>
    private static string? ToSafeExternalUrl(string url)
    {
        url = url.Trim(UrlWhiteSpaceChars);

        if (url.Length == 0)
        {
            return null;
        }

        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        return HasUrlScheme(url) ? null : "http://" + url;
    }

    /// <summary>
    /// Checks if the URL starts with a scheme followed by a colon, the same way as browsers do (<c>ALPHA *( ALPHA / DIGIT / "+" / "-" / "." ) ":"</c>).
    /// </summary>
    /// <remarks>
    /// Anything that is not a valid scheme (like leading whitespace or a tab inside <c>java&#9;script:</c>) counts as no scheme,
    /// so the URL then gets <c>http://</c> in front, and the browser cannot read it as another scheme.
    /// </remarks>
    private static bool HasUrlScheme(string url)
    {
        if (!IsAsciiLetter(url[0]))
        {
            return false;
        }

        for (var i = 1; i < url.Length; i++)
        {
            var c = url[i];

            if (c == ':')
            {
                return true;
            }

            if (!IsAsciiLetter(c) && c is not (>= '0' and <= '9') and not '+' and not '-' and not '.')
            {
                return false;
            }
        }

        return false;
    }

    private static bool IsAsciiLetter(char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');

    /// <summary>
    /// Appends the text encoded for HTML, with runs of Font Awesome icons wrapped in <paramref name="iconStartTag"/> if it is not null.
    /// </summary>
    private static void AppendHtmlText(StringBuilder output, string text, string? iconStartTag)
    {
        var iconStart = iconStartTag is null ? -1 : IndexOfFontAwesomeIcon(text, 0);

        if (iconStart == -1)
        {
            output.Append(WebUtility.HtmlEncode(text));
            return;
        }

        var textStart = 0;

        while (iconStart != -1)
        {
            var iconEnd = iconStart + 1;

            while (iconEnd < text.Length && IsFontAwesomeIcon(text[iconEnd]))
            {
                iconEnd++;
            }

            output.Append(WebUtility.HtmlEncode(text.Substring(textStart, iconStart - textStart)));
            output.Append(iconStartTag);
            output.Append(text, iconStart, iconEnd - iconStart);
            output.Append("</span>");

            textStart = iconEnd;
            iconStart = IndexOfFontAwesomeIcon(text, iconEnd);
        }

        output.Append(WebUtility.HtmlEncode(text.Substring(textStart)));
    }

    private static int IndexOfFontAwesomeIcon(string text, int startIndex)
    {
        for (var i = startIndex; i < text.Length; i++)
        {
            if (IsFontAwesomeIcon(text[i]))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Appends the text from <paramref name="index"/> up to the next dollar sign or carriage return, in one go.
    /// </summary>
    /// <returns>The index after the appended text.</returns>
    private static int AppendText(StringBuilder output, string input, int index, bool uppercase)
    {
        // The text between formatting codes is mostly short, where a simple loop is faster than IndexOfAny
        var end = index + 1;

        while (end < input.Length && input[end] is not ('$' or '\r'))
        {
            end++;
        }

        if (uppercase)
        {
            for (var i = index; i < end; i++)
            {
                output.Append(char.ToUpperInvariant(input[i]));
            }
        }
        else if (end - index == 1)
        {
            output.Append(input[index]);
        }
        else
        {
            output.Append(input, index, end - index);
        }

        return end;
    }

    /// <summary>
    /// Reads the formatting code starting at the dollar sign at <paramref name="index"/>,
    /// the same way as <c>CFastStringFormatting::GetMarkup</c> in ManiaPlanet.
    /// </summary>
    private static Markup ReadMarkup(string input, int index, bool inLink)
    {
        var codeIndex = index + 1;

        // A dollar sign at the end of the string is not displayed
        if (codeIndex >= input.Length)
        {
            return new Markup(MarkupType.Ignored, 1);
        }

        var code = input[codeIndex];

        switch (code)
        {
            case '$':
            case '[':
            case ']':
                return new Markup(MarkupType.Literal, 2, literal: code);
            case '<': return new Markup(MarkupType.Push, 2);
            case '>': return new Markup(MarkupType.Pop, 2);
            case 'g' or 'G': return new Markup(MarkupType.DefaultColor, 2);
            case 'i' or 'I': return new Markup(MarkupType.Italic, 2);
            case 'm' or 'M': return new Markup(MarkupType.Medium, 2);
            case 'n' or 'N': return new Markup(MarkupType.Narrow, 2);
            case 'o' or 'O': return new Markup(MarkupType.Bold, 2);
            case 's' or 'S': return new Markup(MarkupType.Shadow, 2);
            case 't' or 'T': return new Markup(MarkupType.Uppercase, 2);
            case 'u' or 'U': return new Markup(MarkupType.Underline, 2);
            case 'w' or 'W': return new Markup(MarkupType.Wide, 2);
            case 'z' or 'Z': return new Markup(MarkupType.Reset, 2);
            case 'h' or 'H' or 'l' or 'L' or 'p' or 'P':
                // Any link code closes the current link, regardless of its type
                return inLink ? new Markup(MarkupType.LinkEnd, 2) : ReadLinkStart(input, index, code);
        }

        if (char.IsHighSurrogate(code))
        {
            // The game reads a surrogate pair as one character, so the whole pair is the unknown code
            return new Markup(MarkupType.Ignored, codeIndex + 1 < input.Length ? 3 : 2);
        }

        if (!IsHex(code))
        {
            // Unknown codes are not displayed
            return new Markup(MarkupType.Ignored, 2);
        }

        // 1 to 3 hex digits, read by ReadColor when needed
        var length = 2;

        while (length < 4 && index + length < input.Length && IsHex(input[index + length]))
        {
            length++;
        }

        return new Markup(MarkupType.Color, length);
    }

    /// <summary>
    /// Reads the color of the color code at <paramref name="index"/> as a 12-bit number like <c>0xF00</c>. Missing color channels are zero, like in the game.
    /// </summary>
    private static int ReadColor(string input, int index, int length)
    {
        var r = HexValue(input[index + 1]);
        var g = length > 2 ? HexValue(input[index + 2]) : 0;
        var b = length > 3 ? HexValue(input[index + 3]) : 0;

        return (r << 8) | (g << 4) | b;
    }

    private static Markup ReadLinkStart(string input, int index, char code)
    {
        var linkType = code is 'l' or 'L' ? LinkType.External : LinkType.Manialink;
        var afterCode = index + 2;

        if (afterCode < input.Length && input[afterCode] == '[')
        {
            var urlStart = afterCode + 1;
            var urlEnd = input.IndexOf(']', urlStart);

            // Without a closing bracket, the rest of the string is the URL and it stays visible
            if (urlEnd == -1)
            {
                return new Markup(MarkupType.LinkStart, 3, linkType: linkType, linkUrlStart: urlStart, linkUrlLength: input.Length - urlStart);
            }

            return new Markup(MarkupType.LinkStart, urlEnd - index + 1, linkType: linkType, linkUrlStart: urlStart, linkUrlLength: urlEnd - urlStart);
        }

        // Without brackets, the visible text is the URL, found by GetLinkUrl only when needed
        return new Markup(MarkupType.LinkStart, 2, linkType: linkType, linkUrlStart: afterCode, linkUrlLength: -1);
    }

    private static string GetLinkUrl(string input, Markup markup)
    {
        var length = markup.LinkUrlLength >= 0
            ? markup.LinkUrlLength
            : FindLinkTextEnd(input, markup.LinkUrlStart) - markup.LinkUrlStart;

        return input.Substring(markup.LinkUrlStart, length);
    }

    /// <summary>
    /// Finds where the text of a link without brackets ends: before <c>$&lt;</c>, <c>$&gt;</c>, or any link code.
    /// </summary>
    private static int FindLinkTextEnd(string input, int startIndex)
    {
        var afterDollar = false;

        for (var i = startIndex; i < input.Length; i++)
        {
            var c = input[i];

            if (c == '$')
            {
                afterDollar = true;
                continue;
            }

            if (afterDollar && c is '<' or '>' or 'h' or 'H' or 'l' or 'L' or 'p' or 'P')
            {
                return i - 1;
            }

            afterDollar = false;
        }

        return input.Length;
    }

    private static bool IsHex(char c) => (c >= '0' && c <= '9') ||
                                         (c >= 'a' && c <= 'f') ||
                                         (c >= 'A' && c <= 'F');

    /// <summary>
    /// Returns the value of a hex digit that passed <see cref="IsHex(char)"/>.
    /// </summary>
    private static int HexValue(char c) => c <= '9' ? c - '0' : (c | 0x20) - 'a' + 10;

    private enum MarkupType
    {
        Ignored,
        Literal,
        Color,
        DefaultColor,
        Push,
        Pop,
        Reset,
        Italic,
        Bold,
        Shadow,
        Underline,
        Uppercase,
        Wide,
        Medium,
        Narrow,
        LinkStart,
        LinkEnd
    }

    private enum LinkType
    {
        External,
        Manialink
    }

    private enum TextWidth
    {
        Medium,
        Narrow,
        Wide
    }

    /// <summary>
    /// A formatting code. Its color and link URL are not copied out of the input, so that the formatters that do not need them do not allocate.
    /// </summary>
    private readonly struct Markup
    {
        public MarkupType Type { get; }
        public int Length { get; }
        public char Literal { get; }
        public LinkType LinkType { get; }

        /// <summary>
        /// Where the URL of a link start begins in the input.
        /// </summary>
        public int LinkUrlStart { get; }

        /// <summary>
        /// The length of the URL of a link start, or -1 for a link without brackets, whose URL is its visible text.
        /// </summary>
        public int LinkUrlLength { get; }

        public Markup(MarkupType type, int length, char literal = '\0', LinkType linkType = default, int linkUrlStart = 0, int linkUrlLength = 0)
        {
            Type = type;
            Length = length;
            Literal = literal;
            LinkType = linkType;
            LinkUrlStart = linkUrlStart;
            LinkUrlLength = linkUrlLength;
        }
    }

    private struct AnsiState
    {
        public const int NoColor = -1;

        // Stored as color + 1, so that the default state has no color
        private int colorPlusOne;

        /// <summary>
        /// A 12-bit color like <c>0xF00</c>, or <see cref="NoColor"/>.
        /// </summary>
        public int Color
        {
            readonly get => colorPlusOne - 1;
            set => colorPlusOne = value + 1;
        }

        public bool InLink;
        public bool Uppercase;
    }

    private struct HtmlStyle
    {
        public bool Bold;
        public bool Italic;
        public bool Uppercase;
        public bool DropShadow;
        public TextWidth Width;

        /// <summary>
        /// Where the hex digits of the color are in the input, and how many of them there are (0 for no color).
        /// </summary>
        public int ColorIndex;
        public int ColorLength;

        public bool InLink;
        public string? LinkUrl;

        public readonly bool HasCss => ColorLength > 0 || Bold || Italic || Uppercase || DropShadow || Width == TextWidth.Narrow;
    }
}
