using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TmEssentials
{
    public static class Formatter
    {
        private static readonly Regex deformatRegex =
            new Regex("\\$[wnoitsgz><]|\\$[lh]\\[.+\\]|\\$[lh]|\\$[0-9a-f]{1,3}",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // Credits to reaby for the pattern (https://github.com/reaby)

        public static string Deformat(string str)
        {
            return deformatRegex.Replace(str, "");
        }
    }
}
