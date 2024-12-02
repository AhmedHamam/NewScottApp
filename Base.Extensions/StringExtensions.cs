using System.Text;
using System.Text.RegularExpressions;

namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for string operations
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if a string is null, empty, or consists only of white-space characters
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>True if the string is null, empty, or whitespace; otherwise, false</returns>
        public static bool IsNullOrWhiteSpace(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Converts a string to Title Case
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The string in Title Case</returns>
        public static string ToTitleCase(this string str)
        {
            if (str.IsNullOrWhiteSpace()) return str;
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Truncates a string to a specified length
        /// </summary>
        /// <param name="str">The string to truncate</param>
        /// <param name="maxLength">The maximum length</param>
        /// <param name="suffix">The suffix to add to truncated strings</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string str, int maxLength, string suffix = "...")
        {
            if (str == null) return string.Empty;
            if (str.Length <= maxLength) return str;
            return str.Substring(0, maxLength - suffix.Length) + suffix;
        }

        /// <summary>
        /// Removes all HTML tags from a string
        /// </summary>
        /// <param name="html">The HTML string</param>
        /// <returns>The string with HTML tags removed</returns>
        public static string StripHtml(this string html)
        {
            if (html.IsNullOrWhiteSpace()) return html;
            return Regex.Replace(html, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Converts a string to a URL-friendly slug
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The URL-friendly slug</returns>
        public static string ToSlug(this string str)
        {
            if (str.IsNullOrWhiteSpace()) return string.Empty;

            str = str.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Replace(" ", "-");
            return str;
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="str">The string to reverse</param>
        /// <returns>The reversed string</returns>
        public static string Reverse(this string str)
        {
            if (str.Length <= 1) return str;
            var chars = str.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Checks if a string contains only numeric characters
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>True if the string contains only numbers; otherwise, false</returns>
        public static bool IsNumeric(this string str)
        {
            return !string.IsNullOrEmpty(str) && str.All(char.IsDigit);
        }

        /// <summary>
        /// Removes all special characters from a string
        /// </summary>
        /// <param name="str">The string to clean</param>
        /// <returns>The string with special characters removed</returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            if (str.IsNullOrWhiteSpace()) return str;
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Converts a string to Base64
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The Base64 encoded string</returns>
        public static string ToBase64(this string str)
        {
            if (str.IsNullOrWhiteSpace()) return str;
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Converts a Base64 string back to a regular string
        /// </summary>
        /// <param name="base64">The Base64 string</param>
        /// <returns>The decoded string</returns>
        public static string FromBase64(this string base64)
        {
            if (base64.IsNullOrWhiteSpace()) return base64;
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Checks if a string is a valid email address
        /// </summary>
        /// <param name="email">The string to check</param>
        /// <returns>True if the string is a valid email address; otherwise, false</returns>
        public static bool IsValidEmail(this string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts all numbers from a string
        /// </summary>
        /// <param name="str">The string to process</param>
        /// <returns>A string containing only the numbers from the input string</returns>
        public static string ExtractNumbers(this string str)
        {
            if (str.IsNullOrWhiteSpace()) return string.Empty;
            return new string(str.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Checks if a string matches a wildcard pattern
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="pattern">The wildcard pattern (e.g., "*.txt")</param>
        /// <returns>True if the string matches the pattern; otherwise, false</returns>
        public static bool MatchesWildcard(this string str, string pattern)
        {
            if (str == null || pattern == null) return false;
            
            pattern = "^" + Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$";
                
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// Converts a string to camelCase
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The string in camelCase</returns>
        public static string ToCamelCase(this string str)
        {
            if (str.IsNullOrWhiteSpace()) return str;
            var words = str.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return result;
        }
    }
}
