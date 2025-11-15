namespace Utils
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class CaseUtils
    {
        /// <summary>
        /// Converts string to PascalCase (e.g. "hello world" -> "HelloWorld")
        /// </summary>
        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            string[] words = Regex.Split(input, @"[^a-zA-Z0-9]+");

            StringBuilder sb = new StringBuilder();
            foreach (var word in words)
            {
                if (word.Length > 0)
                    sb.Append(textInfo.ToTitleCase(word.ToLowerInvariant()));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts string to camelCase (e.g. "hello world" -> "helloWorld")
        /// </summary>
        public static string ToCamelCase(string input)
        {
            var pascal = ToPascalCase(input);
            if (string.IsNullOrEmpty(pascal)) return string.Empty;

            return char.ToLowerInvariant(pascal[0]) + pascal.Substring(1);
        }

        /// <summary>
        /// Converts string to snake_case (e.g. "hello world" -> "hello_world")
        /// </summary>
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string normalized = Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2");
            normalized = Regex.Replace(normalized, @"[^a-zA-Z0-9]+", "_");
            return normalized.ToLowerInvariant().Trim('_');
        }

        /// <summary>
        /// Converts string to kebab-case (e.g. "hello world" -> "hello-world")
        /// </summary>
        public static string ToKebabCase(string input)
        {
            return ToSnakeCase(input).Replace("_", "-");
        }

        /// <summary>
        /// Converts string to Title Case (e.g. "hello world" -> "Hello World")
        /// </summary>
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLowerInvariant());
        }

        /// <summary>
        /// Converts string to CONSTANT_CASE (e.g. "hello world" -> "HELLO_WORLD")
        /// </summary>
        public static string ToConstantCase(string input)
        {
            return ToSnakeCase(input).ToUpperInvariant();
        }
    }

}