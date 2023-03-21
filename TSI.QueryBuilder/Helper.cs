using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TSI.QueryBuilder
{
    public static class Helper
    {
        public static List<string> ExpandExpression(string expression)
        {
            var regex = @"^(?:\w+\.){1,2}{(.*)}";
            var match = Regex.Match(expression, regex);

            if (!match.Success)
            {
                return new List<string> { expression };
            }

            var table = expression.Substring(0, expression.IndexOf(".{"));

            var captures = match.Groups[1].Value;

            var cols = Regex.Split(captures, @"\s*,\s*")
                .Select(x => $"{table}.{x.Trim()}")
                .ToList();

            return cols;
        }
    }
}
