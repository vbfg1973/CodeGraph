using System.Text.RegularExpressions;

namespace CodeGraph.Domain
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Splits string on capitals. Will honour consecutive capitals such as "USA" as full
        ///     words and not split on those
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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