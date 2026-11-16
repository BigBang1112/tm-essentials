using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials.Benchmarks;

/// <summary>
/// The same parsing rules as <see cref="TextFormatter"/>, with the formatting codes found by a regex without capture groups,
/// enumerated as allocation-free <see cref="ValueMatch"/>es. The kind of code is decided from the character after the dollar sign.
/// Exists only to compare the performance of both approaches.
/// </summary>
internal static partial class RegexValueMatchTextFormatter
{
    private const int MaxStyleStackCount = 7;

    [GeneratedRegex(TextFormatter.FormattingCodeRegexPattern)]
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
        bool restart;

        do
        {
            restart = false;

            foreach (var match in regex.EnumerateMatches(input, position))
            {
                AppendText(output, input, position, match.Index - position, uppercase: false);
                position = match.Index + match.Length;

                if (maxReplacementCount >= 0 && replacementCount >= maxReplacementCount)
                {
                    output.Append(input, match.Index, input.Length - match.Index);
                    return output.ToString();
                }

                replacementCount++;

                // A dollar sign at the end of the string
                if (match.Length == 1)
                {
                    continue;
                }

                var code = input[match.Index + 1];

                switch (code)
                {
                    case '$' or '[' or ']':
                        output.Append(code);
                        break;
                    case 'h' or 'H' or 'l' or 'L' or 'p' or 'P':
                        var next = ApplyLink(input, match, ref inLink);

                        if (next != position)
                        {
                            position = next;
                            restart = true;
                        }
                        break;
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

                if (restart)
                {
                    break;
                }
            }
        }
        while (restart);

        AppendText(output, input, position, input.Length - position, uppercase: false);

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
        bool restart;

        do
        {
            restart = false;

            foreach (var match in regex.EnumerateMatches(input, position))
            {
                AppendText(output, input, position, match.Index - position, state.Uppercase);
                position = match.Index + match.Length;

                if (match.Length == 1)
                {
                    continue;
                }

                var code = input[match.Index + 1];

                switch (code)
                {
                    case '$' or '[' or ']':
                        output.Append(code);
                        break;
                    case 'h' or 'H' or 'l' or 'L' or 'p' or 'P':
                        var next = ApplyLink(input, match, ref state.InLink);

                        if (next != position)
                        {
                            position = next;
                            restart = true;
                        }
                        break;
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
                    default:
                        if (char.IsAsciiHexDigit(code))
                        {
                            state.Color = input.Substring(match.Index + 1, match.Length - 1).PadRight(3, '0');
                            AppendAnsiColor(output, state.Color);
                        }
                        break;
                }

                if (restart)
                {
                    break;
                }
            }
        }
        while (restart);

        AppendText(output, input, position, input.Length - position, state.Uppercase);
        output.Append(TextFormatter.AnsiDefault);

        return output.ToString();
    }

    /// <returns>The position to continue matching from.</returns>
    private static int ApplyLink(string input, ValueMatch match, ref bool inLink)
    {
        if (inLink)
        {
            // Any link code closes the link. What follows the code is parsed again as normal text.
            inLink = false;
            return match.Index + 2;
        }

        inLink = true;

        // Without a closing bracket, the text after the opening bracket stays visible
        if (match.Length > 2 && input[match.Index + match.Length - 1] != ']')
        {
            return match.Index + 3;
        }

        return match.Index + match.Length;
    }

    /// <summary>
    /// Appends the text between formatting codes, without carriage returns.
    /// </summary>
    private static void AppendText(StringBuilder output, string input, int start, int length, bool uppercase)
    {
        var end = start + length;

        if (!uppercase)
        {
            while (start < end)
            {
                var carriageReturn = input.IndexOf('\r', start, end - start);

                if (carriageReturn == -1)
                {
                    output.Append(input, start, end - start);
                    return;
                }

                output.Append(input, start, carriageReturn - start);
                start = carriageReturn + 1;
            }

            return;
        }

        for (var i = start; i < end; i++)
        {
            if (input[i] != '\r')
            {
                output.Append(char.ToUpperInvariant(input[i]));
            }
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
