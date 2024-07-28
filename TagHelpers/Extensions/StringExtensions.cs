using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TagHelpers.Extensions
{
    public static class StringExtensions
    {
        public static string CamelCaseToTitle(this string source, bool removeId = true)
        {
            if(source.Contains("_"))
                return source.Replace("_", " ");
            var rSplit = Regex.Split(source, @"(?<!^)(?=[A-Z])");
            var cnt = rSplit.Length;
            if (removeId && cnt > 1 && rSplit[cnt - 1].Equals("id", StringComparison.OrdinalIgnoreCase))
                cnt -= 1;
            return string.Join(" ", rSplit, 0, cnt);
        }

    }
}
