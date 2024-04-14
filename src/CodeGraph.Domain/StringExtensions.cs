using System.Text.RegularExpressions;

namespace CodeGraph.Domain
{
    public static partial class StringExtensions
    {
        public static IEnumerable<string> SplitStringOnCapitals(this string str)
        {
            Regex regex = SplitStringRegex();

            return regex.Replace(str, "---").Split("---");
        }

        [GeneratedRegex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace)]
        private static partial Regex SplitStringRegex();
    }
}