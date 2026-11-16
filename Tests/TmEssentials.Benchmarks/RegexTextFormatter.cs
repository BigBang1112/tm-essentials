using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials.Benchmarks;

/// <summary>
/// The same parsing rules as <see cref="TextFormatter"/>, but with the formatting codes found by a regex.
/// Exists only to compare the performance of both approaches.
/// </summary>
internal static partial class RegexTextFormatter
{
    private const int MaxStyleStackCount = 7;

    private const int LiteralGroup = 1;
    private const int ColorGroup = 2;
    private const int LinkGroup = 3;
    private const int LinkUrlGroup = 4;
    private const int LinkCloseGroup = 5;
    private const int CodeGroup = 6;

    // $ followed by: literal ($, [, ]), 1-3 hex digit color, link code with optional [url], any other code (a surrogate pair counts as one), or nothing
    [GeneratedRegex(@"\$(?:([$\[\]])|([0-9a-fA-F]{1,3})|([hlpHLP])(?:\[([^\]]*)(\])?)?|([\uD800-\uDBFF][\s\S]?|[\s\S]))?|\r")]
    private static partial Regex MarkupRegex();

    public static string Deformat(string input, int maxReplacementCount = -1)
    {
        ArgumentNullException.ThrowIfNull(input);

        var regex = MarkupRegex();
        var output = new StringBuilder(input.Length);
        var inLink = false;
        var savedLinkStates = new Stack<bool>();
        var replacementCount = 0;
        var position = 0;

        while (position < input.Length)
        {
            var match = regex.Match(input, position);

            if (!match.Success)
            {
                output.Append(input, position, input.Length - position);
                break;
            }

            output.Append(input, position, match.Index - position);
            position = match.Index + match.Length;

            if (input[match.Index] == '\r')
            {
                continue;
            }

            if (maxReplacementCount >= 0 && replacementCount >= maxReplacementCount)
            {
                output.Append(input, match.Index, input.Length - match.Index);
                break;
            }

            replacementCount++;

            var groups = match.Groups;

            if (groups[LiteralGroup].Success)
            {
                output.Append(input[groups[LiteralGroup].Index]);
            }
            else if (groups[LinkGroup].Success)
            {
                position = ApplyLink(match, ref inLink, position);
            }
            else if (groups[CodeGroup].Success)
            {
                switch (input[groups[CodeGroup].Index])
                {
                    case '<':
                        if (savedLinkStates.Count < MaxStyleStackCount) savedLinkStates.Push(inLink);
                        break;
                    case '>':
                        if (savedLinkStates.Count > 0) inLink = savedLinkStates.Pop();
                        break;
                    case 'z' or 'Z':
                        inLink = savedLinkStates.Count > 0 && savedLinkStates.Peek();
                        break;
                }
            }
        }

        return output.ToString();
    }

    public static string FormatAnsi(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var regex = MarkupRegex();
        var output = new StringBuilder();
        var state = default(AnsiState);
        var savedStates = new Stack<AnsiState>();
        var position = 0;

        while (position < input.Length)
        {
            var match = regex.Match(input, position);

            if (!match.Success)
            {
                AppendText(output, input, position, input.Length - position, state.Uppercase);
                break;
            }

            AppendText(output, input, position, match.Index - position, state.Uppercase);
            position = match.Index + match.Length;

            if (input[match.Index] == '\r')
            {
                continue;
            }

            var groups = match.Groups;

            if (groups[LiteralGroup].Success)
            {
                output.Append(input[groups[LiteralGroup].Index]);
            }
            else if (groups[ColorGroup].Success)
            {
                state.Color = groups[ColorGroup].Value.PadRight(3, '0');
                AppendAnsiColor(output, state.Color);
            }
            else if (groups[LinkGroup].Success)
            {
                position = ApplyLink(match, ref state.InLink, position);
            }
            else if (groups[CodeGroup].Success)
            {
                switch (input[groups[CodeGroup].Index])
                {
                    case 'g' or 'G':
                        state.Color = savedStates.Count > 0 ? savedStates.Peek().Color : null;
                        AppendAnsiColor(output, state.Color);
                        break;
                    case 'z' or 'Z':
                        state = savedStates.Count > 0 ? savedStates.Peek() : default;
                        AppendAnsiColor(output, state.Color);
                        break;
                    case 't' or 'T':
                        state.Uppercase = !state.Uppercase;
                        break;
                    case '<':
                        if (savedStates.Count < MaxStyleStackCount)
                        {
                            savedStates.Push(state);
                        }
                        break;
                    case '>':
                        if (savedStates.Count > 0)
                        {
                            state = savedStates.Pop();
                            AppendAnsiColor(output, state.Color);
                        }
                        break;
                }
            }
        }

        output.Append(TextFormatter.AnsiDefault);

        return output.ToString();
    }

    /// <returns>The position to continue matching from.</returns>
    private static int ApplyLink(Match match, ref bool inLink, int position)
    {
        if (inLink)
        {
            // Any link code closes the link. What follows the code is parsed again as normal text.
            inLink = false;
            return match.Index + 2;
        }

        inLink = true;

        // Without a closing bracket, the text after the opening bracket stays visible
        if (match.Groups[LinkUrlGroup].Success && !match.Groups[LinkCloseGroup].Success)
        {
            return match.Index + 3;
        }

        return position;
    }

    private static void AppendText(StringBuilder output, string input, int start, int length, bool uppercase)
    {
        if (!uppercase)
        {
            output.Append(input, start, length);
            return;
        }

        for (var i = start; i < start + length; i++)
        {
            output.Append(char.ToUpperInvariant(input[i]));
        }
    }

    private struct AnsiState
    {
        public string? Color;
        public bool InLink;
        public bool Uppercase;
    }

    private static void AppendAnsiColor(StringBuilder output, string? color)
    {
        if (color is null)
        {
            output.Append(TextFormatter.AnsiDefault);
            return;
        }

        TextFormatter.AppendAnsiText(output, "$" + color);
    }
}
