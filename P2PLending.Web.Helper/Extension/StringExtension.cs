using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace P2PLending.Web.Helper.Extension
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string input)
        {
            var result = input.Replace("_", "");
            if (result.Length == 0) return "null";
            result = Regex.Replace(result, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            return char.ToLower(result[0]) + result.Substring(1);
        }
        public static string ToPascalCase(this string input)
        {
            var result = input.Replace("_", "");
            if (result.Length == 0) return "null";
            result = Regex.Replace(result, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            return char.ToUpper(result[0]) + result.Substring(1);
        }
        public static string ToSnakeCase(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (input.Length < 2)
            {
                return input;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(input[0]));
            for (int i = 1; i < input.Length; ++i)
            {
                char c = input[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
