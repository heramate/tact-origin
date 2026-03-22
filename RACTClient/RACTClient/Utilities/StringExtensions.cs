using System;
using System.Text.RegularExpressions;

namespace RACTClient.Utilities
{
    public static class StringExtensions
    {
        public static string ToSingleSpace(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }
    }
}
