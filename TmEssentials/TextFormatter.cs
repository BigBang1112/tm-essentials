using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials;

public static class TextFormatter
{
    //
    // Credits to reaby for the ANSII stuff (https://github.com/reaby)
    //

    private static readonly Regex deformatRegex =
        new(@"\$((\$)|[0-9a-f]{2,3}|[lh]\[.*?\]|.)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex colorRegex =
        new(@"\$[0-9a-f]{1,3}",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private const string AnsiDefault = "\x1B[39m\x1B[22m";

    public static string Deformat(string input)
    {
        return deformatRegex.Replace(input, "$2");
    }

    public static string FormatAnsi(string input)
    {
        var output = new StringBuilder();

        var split = deformatRegex.Split(input);

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
