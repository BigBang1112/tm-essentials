using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials;

public static partial class TextFormatter
{
    //private const string DeformatRegexForAnsiPattern = @"\$([0-9a-f]{1,3})|\$(\$)|\$[lh]\[.*?\]|\$[lh]|(\$z)|\$.|([^$]+)";
    private const string DeformatRegexOldPattern = @"(\$[0-9a-f]{1,3}|\$[lh]\[.+\]|\$[lh]|\$.)";
    private const string DeformatRegexPattern = @"\$((\$)|[0-9a-f]{2,3}|[lh]\[.*?\]|.)";
    private const string ColorRegexPattern = @"\$[0-9a-f]{1,3}";
    
    private const RegexOptions RegexOpts = RegexOptions.IgnoreCase | RegexOptions.Compiled;

    //
    // Credits to reaby for the ANSII stuff (https://github.com/reaby)
    //


#if NET7_0_OR_GREATER
    //[RegexGenerator(DeformatRegexForAnsiPattern, RegexOpts)]
    //private static partial Regex DeformatRegexForAnsi();

    [RegexGenerator(DeformatRegexOldPattern, RegexOpts)]
    private static partial Regex DeformatRegexOld();

    [RegexGenerator(DeformatRegexPattern, RegexOpts)]
    private static partial Regex DeformatRegex();

    [RegexGenerator(ColorRegexPattern, RegexOpts)]
    private static partial Regex ColorRegex();

    //private static readonly Regex deformatRegexForAnsi = DeformatRegexForAnsi();
    private static readonly Regex deformatRegexOld = DeformatRegexOld();
    private static readonly Regex deformatRegex = DeformatRegex();
    private static readonly Regex colorRegex = ColorRegex();
#else
    //private static readonly Regex deformatRegexForAnsi = new(DeformatRegexForAnsiPattern, RegexOpts);
    private static readonly Regex deformatRegexOld = new(DeformatRegexOldPattern, RegexOpts);
    private static readonly Regex deformatRegex = new(DeformatRegexPattern, RegexOpts);
    private static readonly Regex colorRegex = new(ColorRegexPattern, RegexOpts);
#endif

    private const string AnsiDefault = "\x1B[39m\x1B[22m";

    public static string Deformat(string input)
    {
        return deformatRegex.Replace(input, "$2");
    }

    // This could be potentially fast, but the memory allocation is doubled by using this regex
    // So far this waits for memory allocation improvements regarding regex (which likely won't ever happen)
    /*
    public static string FormatAnsi(string input)
    {
        var output = new StringBuilder();

        var matches = deformatRegexForAnsi.Matches(input);

        foreach (Match match in matches)
        {
            output.Append(ColorToAnsi(match));
        }

        output.Append(AnsiDefault);

        return output.ToString();
    }

    private static string ColorToAnsi(Match match)
    {
        if (match.Groups[4].Success)
        {
            return match.Value;
        }

        return "";

        var colorGroup = match.Groups[1];

        if (!colorGroup.Success)
        {
            var isReset = match.Groups[3].Success;

            if (isReset)
            {
                return AnsiDefault;
            }

            return "";
        }

        var colorInt16 = Convert.ToInt16(colorGroup.Value, 16);

        var r = 0x11 * ((colorInt16 & 0xF00) >> 8);
        var g = 0x11 * ((colorInt16 & 0x0F0) >> 4);
        var b = 0x11 * (colorInt16 & 0x00F);

        Color color = Color.FromArgb(r, g, b);

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

        return $"\x1B[{boldAttr};{colorAttr}m";
    }*/

    public static string FormatAnsi(string input)
    {
        var output = new StringBuilder();

        var split = deformatRegexOld.Split(input);

        foreach (string text in split)
        {
            output.Append(ColorToAnsi(text));
        }

        output.Append(AnsiDefault);

        return output.ToString();
    }

    private static string ColorToAnsi(string input)
    {
        if (!input.StartsWith("$")) return input;
        if (input.EndsWith("z")) return AnsiDefault;

        if (!colorRegex.IsMatch(input))
        {
            return "";
        }
        
        var colorInt16 = Convert.ToInt16(input.Replace("$", ""), 16);

        var r = 0x11 * ((colorInt16 & 0xF00) >> 8);
        var g = 0x11 * ((colorInt16 & 0x0F0) >> 4);
        var b = 0x11 * (colorInt16 & 0x00F);

        Color color = Color.FromArgb(r, g, b);
        
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

        return $"\x1B[{boldAttr};{colorAttr}m";
    }
}
