using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials;

/// <summary>
/// Provides a set of methods for text formatting in Nadeo games.
/// </summary>
public static partial class TextFormatter
{
    //
    // Credits to reaby for the ANSI stuff (https://github.com/reaby)
    //

#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Regex)]
#endif
    private const string DeformatRegexStr = @"\$(?:(\$)|[0-9a-fA-F]{2,3}|[lhLH]\[.*?\]|[lhLH]\[|.)";

#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Regex)]
#endif
    private const string DeformatRegexOldStr = @"(\$[0-9a-fA-F]{1,3}|\$[lhLH]\[.+\]|\$[lhLH]|\$.)";

#if NET7_0_OR_GREATER
    [GeneratedRegex(DeformatRegexStr)]
    private static partial Regex DeformatRegex();

    [GeneratedRegex(DeformatRegexOldStr)]
    private static partial Regex DeformatRegexOld();
#endif

    // Doesn't work for dollar
    private static readonly Regex deformatRegexOld = 
#if NET7_0_OR_GREATER
        DeformatRegexOld();
#else
        new(DeformatRegexOldStr, RegexOptions.Compiled);
#endif

    private static readonly Regex deformatRegex =
#if NET7_0_OR_GREATER
        DeformatRegex();
#else
        new(DeformatRegexStr, RegexOptions.Compiled);
#endif

    internal const string AnsiDefault = "\x1B[39m\x1B[22m";

    /// <summary>
    /// Deformats a string from Nadeo format.
    /// </summary>
    /// <param name="input">A string input.</param>
    /// <returns>A deformatted string.</returns>
    /// <exception cref="ArgumentNullException">Input is null.</exception>
    /// <exception cref="RegexMatchTimeoutException">Regex match timeout.</exception>
    public static string Deformat(string input)
    {
        return deformatRegex.Replace(input, "$1");
    }

    /// <summary>
    /// Deformats a string from Trackmania/Shootmania format.
    /// </summary>
    /// <param name="input">A string input.</param>
    /// <param name="maxReplacementCount">The maximum number of deformat actions (replacements) to make.</param>
    /// <returns>A deformatted string.</returns>
    /// <exception cref="ArgumentNullException">Input is null.</exception>
    /// <exception cref="RegexMatchTimeoutException">Regex match timeout.</exception>
    public static string Deformat(string input, int maxReplacementCount)
    {
        return deformatRegex.Replace(input, "$1", maxReplacementCount);
    }

    /// <summary>
    /// Formats a string to ANSI format.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Input is null.</exception>
    /// <exception cref="RegexMatchTimeoutException">Regex match timeout.</exception>
    public static string FormatAnsi(string input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var output = new StringBuilder();
        
        var split = deformatRegexOld.Split(input);

        foreach (var element in split)
        {
            AppendAnsiText(output, element);
        }

        output.Append(AnsiDefault);

        return output.ToString();
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
    /// <param name="allowManialinks">Whether manialinks ($h) should be processed.</param>
    /// <returns>A Nadeo string formatted as HTML.</returns>
    public static string FormatHtml(string? input, bool allowLinks = false, bool allowManialinks = false)
    {
        if (input is null or "")
        {
            return string.Empty;
        }

        var segments = new List<Segment>();
        var styleStack = new Stack<Style>();
        var currentStyle = new Style();
        var textBuffer = new StringBuilder();
        var linkStack = new Stack<LinkInfo>();

        void FlushBuffer()
        {
            if (textBuffer.Length > 0)
            {
                var segment = new Segment { Text = textBuffer.ToString(), Style = currentStyle.Clone() };
                if (linkStack.Count > 0)
                {
                    var linkInfo = linkStack.Peek();
                    segment.LinkUrl = linkInfo.IsManialink ? $"maniaplanet:///:{linkInfo.Url}" : linkInfo.Url;
                }
                segments.Add(segment);
                textBuffer.Clear();
            }
        }

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '$' && i + 1 < input.Length)
            {
                char code = char.ToLowerInvariant(input[i + 1]);

                // Literal $$
                if (code == '$')
                {
                    textBuffer.Append('$');
                    i++;
                    continue;
                }

                // Color code: 3 hex digits
                if (i + 3 < input.Length && IsHex(input[i + 1]) && IsHex(input[i + 2]) && IsHex(input[i + 3]))
                {
                    FlushBuffer();
                    string hex3 = input.Substring(i + 1, 3);
                    currentStyle.Color = $"#{hex3[0]}{hex3[1]}{hex3[2]}";
                    i += 3;
                    continue;
                }

                // Check for link closing tag first
                if (linkStack.Count > 0 && (code == 'l' || code == 'h'))
                {
                    FlushBuffer();
                    linkStack.Pop();

                    // Check if there are brackets with content after the closing tag
                    if (i + 2 < input.Length && input[i + 2] == '[')
                    {
                        int closingBracket = input.IndexOf(']', i + 3);
                        if (closingBracket != -1)
                        {
                            // Keep the brackets and content as regular text
                            string bracketContent = input.Substring(i + 2, closingBracket - (i + 2) + 1);
                            textBuffer.Append(bracketContent);
                            i = closingBracket;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                    continue;
                }

                // Links ($l) and manialinks ($h)
                if ((code == 'l' && allowLinks) || (code == 'h' && allowManialinks))
                {
                    FlushBuffer();

                    // Check if it's bracketed format: $l[url]text$l or $h[url]text$h
                    if (i + 2 < input.Length && input[i + 2] == '[')
                    {
                        int urlStart = i + 3;
                        int urlEnd = input.IndexOf(']', urlStart);
                        if (urlEnd != -1)
                        {
                            string url = input.Substring(urlStart, urlEnd - urlStart);
                            linkStack.Push(new LinkInfo { Url = url, IsManialink = code == 'h' });
                            i = urlEnd;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        // Simple format: $lurl$l or $hurl$h (or until end)
                        int urlStart = i + 2;
                        int urlEnd = FindClosingTag(input, urlStart, code);
                        if (urlEnd == -1)
                            urlEnd = input.Length;

                        string url = input.Substring(urlStart, urlEnd - urlStart);

                        var linkSegment = new Segment
                        {
                            Text = url,
                            Style = currentStyle.Clone(),
                            LinkUrl = code == 'h' ? $"maniaplanet:///:{url}" : url
                        };
                        segments.Add(linkSegment);

                        // Skip the closing tag if found
                        i = urlEnd < input.Length && urlEnd + 1 < input.Length ? urlEnd + 1 : urlEnd - 1;
                    }
                    continue;
                }

                // Links ($l/$h) will be deformatted if links are disabled
                if (code == 'l' || code == 'h')
                {
                    // if next is not [, continue
                    if (i + 2 >= input.Length || input[i + 2] != '[')
                    {
                        FlushBuffer();
                        i++;
                        continue;
                    }

                    // if next is [, find the closing ]
                    int closingBracket = input.IndexOf(']', i + 2);
                    if (closingBracket == -1)
                    {
                        FlushBuffer();
                        i++;
                        continue;
                    }

                    i = closingBracket;
                    continue;
                }

                // Other control codes
                FlushBuffer();
                switch (code)
                {
                    case 'o': currentStyle.Bold = !currentStyle.Bold; break;
                    case 'i': currentStyle.Italic = !currentStyle.Italic; break;
                    case 't': currentStyle.Uppercase = !currentStyle.Uppercase; break;
                    case 's': currentStyle.DropShadow = !currentStyle.DropShadow; break;
                    case 'w': currentStyle.Wide = !currentStyle.Wide; break;
                    case 'n': currentStyle.Narrow = !currentStyle.Narrow; break;
                    case 'g': currentStyle.Color = null; break;
                    case 'z':
                        currentStyle.Bold = currentStyle.Italic = currentStyle.Uppercase =
                        currentStyle.DropShadow = currentStyle.Wide = currentStyle.Narrow = false;
                        linkStack.Clear();
                        break;
                    case '<':
                        styleStack.Push(currentStyle.Clone());
                        break;
                    case '>':
                        if (styleStack.Count > 0)
                        {
                            currentStyle = styleStack.Pop();
                            linkStack.Clear(); // Clear links when popping style stack
                        }
                        break;
                    default:
                        // Unknown or unhandled control: do not display
                        break;
                }
                i++;
            }
            else
            {
                char c = input[i];
                if (currentStyle.Uppercase)
                    c = char.ToUpperInvariant(c);
                textBuffer.Append(c);
            }
        }

        FlushBuffer();

        // Build HTML with inline styles
        var sb = new StringBuilder();
        foreach (var seg in segments)
        {
            string style = seg.Style.ToCss();
            string encoded = WebUtility.HtmlEncode(seg.Text);

            if (!string.IsNullOrEmpty(seg.LinkUrl))
            {
                string linkStyle = string.IsNullOrEmpty(style) ? "" : $" style=\"{style}\"";
                string encodedUrl = WebUtility.HtmlEncode(seg.LinkUrl);
                sb.Append($"<a href=\"{encodedUrl}\"{linkStyle}>{encoded}</a>");
            }
            else if (string.IsNullOrEmpty(style))
            {
                sb.Append(encoded);
            }
            else
            {
                sb.Append($"<span style=\"{style}\">{encoded}</span>");
            }
        }

        return sb.ToString();
    }

    private static bool IsHex(char c) => (c >= '0' && c <= '9') ||
                                         (c >= 'a' && c <= 'f') ||
                                         (c >= 'A' && c <= 'F');

    private static int FindClosingTag(string input, int startIndex, char code)
    {
        for (int i = startIndex; i < input.Length - 1; i++)
        {
            if (input[i] == '$' && i + 1 < input.Length)
            {
                char nextChar = input[i + 1];
                if (nextChar == code || nextChar == char.ToUpperInvariant(code))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private sealed class LinkInfo
    {
        public string Url { get; set; } = string.Empty;
        public bool IsManialink { get; set; }
    }

    private sealed class Segment
    {
        public string Text { get; set; } = string.Empty;
        public Style Style { get; set; } = new Style();
        public string? LinkUrl { get; set; }
    }

    private sealed class Style
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Uppercase { get; set; }
        public bool DropShadow { get; set; }
        public bool Wide { get; set; }
        public bool Narrow { get; set; }
        public string? Color { get; set; }

        // Note: Clone() uses MemberwiseClone(), which performs a shallow copy.
        // If reference-type properties are added to Style, this may cause bugs.
        public Style Clone() => (Style)MemberwiseClone();

        public string ToCss()
        {
            var parts = new List<string>();
            if (Color is not null) parts.Add($"color:{Color}");
            if (Bold) parts.Add("font-weight:bold");
            if (Italic) parts.Add("font-style:italic");
            if (Uppercase) parts.Add("text-transform:uppercase");
            if (DropShadow) parts.Add("text-shadow:1px 1px 2px black");
            if (Narrow) parts.Add("letter-spacing:-0.05em");
            return string.Join(";", parts);
        }
    }
}
