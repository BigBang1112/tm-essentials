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

        if (element[element.Length - 1] == 'z' || element[element.Length - 1] == 'g')
        {
            output.Append(AnsiDefault);
            return;
        }

        if (element.Length <= 3)
        {
            for (var i = 0; i < element.Length; i++)
            {
                if (element[i] < '0' || element[i] > 'F')
                {
                    return;
                }
            }
        }

        var colorInt16 = Convert.ToInt16(element.Substring(1), 16);

        var r = 0x11 * ((colorInt16 & 0xF00) >> 8);
        var g = 0x11 * ((colorInt16 & 0x0F0) >> 4);
        var b = 0x11 * (colorInt16 & 0x00F);

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
    /// Formats a Nadeo string into a HTML format.
    /// </summary>
    /// <param name="input">The input Nadeo string.</param>
    /// <param name="allowLinks">Whether standard links ($l) should be processed.</param>
    /// <param name="allowManialinks">Whether manialinks ($h and $p) should be processed.</param>
    /// <returns>A Nadeo string formatted as HTML.</returns>
    public static string FormatHtml(string? input, bool allowLinks = false, bool allowManialinks = false)
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
                    style.LinkUrl = markup.LinkType switch
                    {
                        LinkType.External when allowLinks => markup.LinkUrl,
                        LinkType.Manialink when allowManialinks => $"maniaplanet:///:{markup.LinkUrl}",
                        _ => null
                    };
                    break;
                case MarkupType.LinkEnd:
                    style.InLink = false;
                    style.LinkUrl = null;
                    break;
            }
        }

        FlushBuffer();

        // Build HTML with inline styles
        var sb = new StringBuilder();
        foreach (var seg in segments)
        {
            var css = seg.Style.ToCss();
            var encoded = WebUtility.HtmlEncode(seg.Text);

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
