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
    /// A regex pattern that matches one formatting code: a dollar sign followed by a link code with <c>[url]</c>,
    /// a color of 1 to 3 hex digits, any other character, or nothing (at the end of the string).
    /// </summary>
    /// <remarks>
    /// The pattern does not depend on regex options. Line breaks are matched like any other character, as in the game.
    /// </remarks>
#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Regex)]
#endif
    public const string FormattingCodeRegexPattern = @"\$(?:[hlpHLP]\[[^\]]*\]?|[0-9a-fA-F]{1,3}|[\uD800-\uDBFF][\s\S]?|[\s\S])?";

    /// <summary>
    /// The game keeps the base style plus up to 7 styles saved by <c>$&lt;</c>. Further <c>$&lt;</c> are ignored.
    /// </summary>
    private const int MaxStyleStackCount = 7;

    /// <summary>
    /// The characters that the game trims around a link URL before opening it (<c>CFastString::s_DefaultWhiteSpaceChars</c>).
    /// </summary>
    private static readonly char[] UrlWhiteSpaceChars = [' ', '\t', '\r', '\n'];

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

        var output = new StringBuilder(input.Length);
        var inLink = false;
        var savedLinkStates = new Stack<bool>();
        var replacementCount = 0;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (c != '$')
            {
                if (c != '\r')
                {
                    output.Append(c);
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
                    if (savedLinkStates.Count < MaxStyleStackCount)
                    {
                        savedLinkStates.Push(inLink);
                    }
                    break;
                case MarkupType.Pop:
                    if (savedLinkStates.Count > 0)
                    {
                        inLink = savedLinkStates.Pop();
                    }
                    break;
                case MarkupType.Reset:
                    inLink = savedLinkStates.Count > 0 && savedLinkStates.Peek();
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

        var output = new StringBuilder();
        var state = default(AnsiState);
        var savedStates = new Stack<AnsiState>();

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (c != '$')
            {
                if (c != '\r')
                {
                    output.Append(state.Uppercase ? char.ToUpperInvariant(c) : c);
                }

                continue;
            }

            var markup = ReadMarkup(input, i, state.InLink);
            i += markup.Length - 1;

            switch (markup.Type)
            {
                case MarkupType.Literal:
                    output.Append(markup.Literal);
                    break;
                case MarkupType.Color:
                    state.Color = markup.Color;
                    AppendAnsiColor(output, state.Color);
                    break;
                case MarkupType.DefaultColor:
                    state.Color = savedStates.Count > 0 ? savedStates.Peek().Color : null;
                    AppendAnsiColor(output, state.Color);
                    break;
                case MarkupType.Reset:
                    state = savedStates.Count > 0 ? savedStates.Peek() : default;
                    AppendAnsiColor(output, state.Color);
                    break;
                case MarkupType.Push:
                    if (savedStates.Count < MaxStyleStackCount)
                    {
                        savedStates.Push(state);
                    }
                    break;
                case MarkupType.Pop:
                    if (savedStates.Count > 0)
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

    private static void AppendAnsiColor(StringBuilder output, string? color)
    {
        if (color is null)
        {
            output.Append(AnsiDefault);
            return;
        }

        AppendAnsiText(output, "$" + color);
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

        // Missing color channels are zero, like in the game ($f is $f00)
        var r = 0x11 * HexValue(element[1]);
        var g = element.Length > 2 ? 0x11 * HexValue(element[2]) : 0;
        var b = element.Length > 3 ? 0x11 * HexValue(element[3]) : 0;

        var color = Color.FromArgb(r, g, b);

        var hue = color.GetHue();

        if (color.GetSaturation() == 0)
        {
            hue = 0;
        }

        var colorAttr = hue switch
        {
            < 30 => 31,// red
            < 80 => 33,// yellow
            < 160 => 32,// green
            < 214 => 36,// cyan
            < 284 => 34,// blue
            < 333 => 35,// magenta
            _ => 31,
        };

        var boldAttr = 0;

        if (color.GetBrightness() < 0.1)
        {
            boldAttr = 1;
            colorAttr = 30;
        }
        else
        {
            if (color.GetBrightness() > 0.43)
            {
                boldAttr = 1;
            }

            if (color.GetBrightness() > 0.9)
            {
                boldAttr = 1;
                colorAttr = 37;
            }
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        ReadOnlySpan<char> span =
        [
            '\x1B',
            '[',
            (char)(boldAttr + 48),
            ';',
            '3',
            (char)(colorAttr + 18),
            'm'
        ];

        output.Append(span);
#else
        output.Append($"\x1B[{boldAttr};{colorAttr}m");
#endif
    }

    /// <summary>
    /// Checks if the character is a Font Awesome 4.7 icon. The game renders these private use characters (U+F000 to U+F2E0) as icons.
    /// </summary>
    /// <param name="c">A character.</param>
    /// <returns>True if the character is a Font Awesome 4.7 icon, otherwise false.</returns>
    public static bool IsFontAwesomeIcon(char c) => c is >= '\uF000' and <= '\uF2E0';

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

        var segments = new List<Segment>();
        var savedStyles = new Stack<Style>();
        var defaultStyle = new Style();
        var style = new Style();
        var textBuffer = new StringBuilder();

        void FlushBuffer()
        {
            if (textBuffer.Length > 0)
            {
                segments.Add(new Segment { Text = textBuffer.ToString(), Style = style.Clone() });
                textBuffer.Clear();
            }
        }

        Style GetSavedStyle() => savedStyles.Count > 0 ? savedStyles.Peek() : defaultStyle;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (c != '$')
            {
                if (c != '\r')
                {
                    textBuffer.Append(style.Uppercase ? char.ToUpperInvariant(c) : c);
                }

                continue;
            }

            var markup = ReadMarkup(input, i, style.InLink);
            i += markup.Length - 1;

            if (markup.Type == MarkupType.Literal)
            {
                textBuffer.Append(markup.Literal);
                continue;
            }

            if (markup.Type is MarkupType.Ignored or MarkupType.Underline)
            {
                continue;
            }

            FlushBuffer();

            switch (markup.Type)
            {
                case MarkupType.Color:
                    style.Color = "#" + markup.Color;
                    break;
                case MarkupType.DefaultColor:
                    style.Color = GetSavedStyle().Color;
                    break;
                case MarkupType.Reset:
                    style = GetSavedStyle().Clone();
                    break;
                case MarkupType.Push:
                    if (savedStyles.Count < MaxStyleStackCount)
                    {
                        savedStyles.Push(style.Clone());
                    }
                    break;
                case MarkupType.Pop:
                    if (savedStyles.Count > 0)
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
                    // The URL of a link start is never null
                    style.LinkUrl = markup.LinkType == LinkType.External
                        ? (allowLinks ? ToSafeExternalUrl(markup.LinkUrl!) : null)
                        : (allowManialinks ? $"maniaplanet:///:{markup.LinkUrl}" : null);
                    break;
                default:
                    // MarkupType.LinkEnd is the only one left, the markups without style were skipped above
                    style.InLink = false;
                    style.LinkUrl = null;
                    break;
            }
        }

        FlushBuffer();

        // Build HTML with inline styles
        var sb = new StringBuilder();
        var iconStartTag = string.IsNullOrEmpty(iconCssClass) ? null : $"<span class=\"{WebUtility.HtmlEncode(iconCssClass)}\">";

        foreach (var seg in segments)
        {
            var css = seg.Style.ToCss();
            var encoded = EncodeHtmlText(seg.Text, iconStartTag);

            if (!string.IsNullOrEmpty(seg.Style.LinkUrl))
            {
                var linkStyle = string.IsNullOrEmpty(css) ? "" : $" style=\"{css}\"";
                var encodedUrl = WebUtility.HtmlEncode(seg.Style.LinkUrl);
                sb.Append($"<a href=\"{encodedUrl}\"{linkStyle}>{encoded}</a>");
            }
            else if (string.IsNullOrEmpty(css))
            {
                sb.Append(encoded);
            }
            else
            {
                sb.Append($"<span style=\"{css}\">{encoded}</span>");
            }
        }

        return sb.ToString();
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
    /// Encodes the text for HTML, with runs of Font Awesome icons wrapped in <paramref name="iconStartTag"/> if it is not null.
    /// </summary>
    private static string EncodeHtmlText(string text, string? iconStartTag)
    {
        if (iconStartTag is null)
        {
            return WebUtility.HtmlEncode(text);
        }

        var iconStart = IndexOfFontAwesomeIcon(text, 0);

        if (iconStart == -1)
        {
            return WebUtility.HtmlEncode(text);
        }

        var output = new StringBuilder();
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

        return output.ToString();
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

        // 1 to 3 hex digits, missing color channels are zero
        var color = new[] { code, '0', '0' };
        var length = 2;

        while (length < 4 && index + length < input.Length && IsHex(input[index + length]))
        {
            color[length - 1] = input[index + length];
            length++;
        }

        return new Markup(MarkupType.Color, length, color: new string(color));
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
                return new Markup(MarkupType.LinkStart, 3, linkType: linkType, linkUrl: input.Substring(urlStart));
            }

            return new Markup(MarkupType.LinkStart, urlEnd - index + 1, linkType: linkType, linkUrl: input.Substring(urlStart, urlEnd - urlStart));
        }

        // Without brackets, the visible text is the URL
        var textEnd = FindLinkTextEnd(input, afterCode);

        return new Markup(MarkupType.LinkStart, 2, linkType: linkType, linkUrl: input.Substring(afterCode, textEnd - afterCode));
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

    private readonly struct Markup
    {
        public MarkupType Type { get; }
        public int Length { get; }
        public char Literal { get; }
        public string? Color { get; }
        public LinkType LinkType { get; }
        public string? LinkUrl { get; }

        public Markup(MarkupType type, int length, char literal = '\0', string? color = null, LinkType linkType = default, string? linkUrl = null)
        {
            Type = type;
            Length = length;
            Literal = literal;
            Color = color;
            LinkType = linkType;
            LinkUrl = linkUrl;
        }
    }

    private struct AnsiState
    {
        public string? Color;
        public bool InLink;
        public bool Uppercase;
    }

    private sealed class Segment
    {
        public string Text { get; set; } = string.Empty;
        public Style Style { get; set; } = new Style();
    }

    private sealed class Style
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Uppercase { get; set; }
        public bool DropShadow { get; set; }
        public TextWidth Width { get; set; }
        public string? Color { get; set; }
        public bool InLink { get; set; }
        public string? LinkUrl { get; set; }

        // Note: Clone() uses MemberwiseClone(), which performs a shallow copy.
        // If mutable reference-type properties are added to Style, this may cause bugs.
        public Style Clone() => (Style)MemberwiseClone();

        public string ToCss()
        {
            var parts = new List<string>();
            if (Color is not null) parts.Add($"color:{Color}");
            if (Bold) parts.Add("font-weight:bold");
            if (Italic) parts.Add("font-style:italic");
            if (Uppercase) parts.Add("text-transform:uppercase");
            if (DropShadow) parts.Add("text-shadow:1px 1px 2px black");
            if (Width == TextWidth.Narrow) parts.Add("letter-spacing:-0.05em");
            return string.Join(";", parts);
        }
    }
}
