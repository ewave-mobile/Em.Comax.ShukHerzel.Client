using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace EM.Comax.ShukHerzl.Common
{
    public static class XmlSanitizer
    {
        private static readonly Regex HexEntityPattern = new(
            @"&#x(0*[0-9a-fA-F]{1,4});",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex DecimalEntityPattern = new(
            @"&#(\d{1,5});",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ScientificNotationPattern = new(
            @"(?<=>)\s*([+-]?(?:\d+\.?\d*|\.\d+)[eE][+-]?\d+)\s*(?=</)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string Sanitize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = HexEntityPattern.Replace(input, ReplaceInvalidEntity);
            input = DecimalEntityPattern.Replace(input, ReplaceInvalidEntity);
            input = NormalizeScientificNotation(input);
            return RemoveInvalidCharacters(input);
        }

        private static string ReplaceInvalidEntity(Match match)
        {
            var value = match.Groups[1].Value;
            var numberStyle = match.Value.StartsWith("&#x", StringComparison.OrdinalIgnoreCase)
                ? NumberStyles.HexNumber
                : NumberStyles.Integer;

            if (!int.TryParse(value, numberStyle, CultureInfo.InvariantCulture, out var codePoint))
                return match.Value;

            return XmlConvert.IsXmlChar((char)codePoint) ? match.Value : " ";
        }

        private static string NormalizeScientificNotation(string input)
        {
            return ScientificNotationPattern.Replace(input, match =>
            {
                var value = match.Groups[1].Value;
                if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
                    return match.Value;

                return ((decimal)number).ToString(CultureInfo.InvariantCulture);
            });
        }

        private static string RemoveInvalidCharacters(string input)
        {
            var sanitized = new StringBuilder(input.Length);

            foreach (var character in input)
            {
                sanitized.Append(XmlConvert.IsXmlChar(character) ? character : ' ');
            }

            return sanitized.ToString();
        }
    }
}
