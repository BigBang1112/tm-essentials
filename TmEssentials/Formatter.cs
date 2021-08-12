using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials
{
    public static class Formatter
    {
        //
        // Credits to reaby for the patterns (https://github.com/reaby)
        //

        private static readonly Regex deformatRegex =
            new Regex("(\\$[wnoitsgz><]|\\$[lh]\\[.+\\]|\\$[lh]|\\$[0-9a-f]{1,3})",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex colorRegex =
            new Regex("\\$[0-9a-f]{1,3}",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private const string AnsiDefault = "\x1B[39m\x1B[22m";

        public static string Deformat(string input)
        {
            return deformatRegex.Replace(input, "");
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

            if (colorRegex.IsMatch(input))
            {
                var colorInt16 = Convert.ToInt16(input.Replace("$", ""), 16);

                var r = 0x11 * ((colorInt16 & 0xF00) >> 8);
                var g = 0x11 * ((colorInt16 & 0x0F0) >> 4);
                var b = 0x11 * (colorInt16 & 0x00F);

                Color color = Color.FromArgb(r, g, b);

                int boldAttr = 0;
                float hue = color.GetHue();
                if (color.GetSaturation() == 0)
                {
                    hue = 0;
                }

                int colorAttr;
                if (hue < 30) colorAttr = 31; // red
                else if (hue < 80) colorAttr = 33; // yellow
                else if (hue < 160) colorAttr = 32; // green
                else if (hue < 214) colorAttr = 36; // cyan
                else if (hue < 284) colorAttr = 34; // blue
                else if (hue < 333) colorAttr = 35; // magenta
                else colorAttr = 31;

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
            return "";
        }
    }
}
