namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for integer types
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Checks if an integer is within a specified range (inclusive)
        /// </summary>
        /// <param name="value">The integer to check</param>
        /// <param name="min">The minimum value of the range</param>
        /// <param name="max">The maximum value of the range</param>
        /// <returns>True if the value is within the range; otherwise, false</returns>
        public static bool IsInRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Ensures a value is within a specified range
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        /// <summary>
        /// Converts an integer to a string with leading zeros
        /// </summary>
        /// <param name="value">The integer to convert</param>
        /// <param name="totalWidth">The total width of the resulting string</param>
        /// <returns>A string representation of the integer with leading zeros</returns>
        public static string ToStringWithLeadingZeros(this int value, int totalWidth)
        {
            return value.ToString($"D{totalWidth}");
        }

        /// <summary>
        /// Checks if an integer is even
        /// </summary>
        /// <param name="value">The integer to check</param>
        /// <returns>True if the value is even; otherwise, false</returns>
        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Checks if an integer is odd
        /// </summary>
        /// <param name="value">The integer to check</param>
        /// <returns>True if the value is odd; otherwise, false</returns>
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        /// <summary>
        /// Returns the ordinal string representation of an integer
        /// </summary>
        /// <param name="value">The integer to convert</param>
        /// <returns>The ordinal string (e.g., "1st", "2nd", "3rd", "4th")</returns>
        public static string ToOrdinal(this int value)
        {
            string suffix = (value % 100) switch
            {
                11 or 12 or 13 => "th",
                _ => (value % 10) switch
                {
                    1 => "st",
                    2 => "nd",
                    3 => "rd",
                    _ => "th"
                }
            };
            return $"{value}{suffix}";
        }

        /// <summary>
        /// Converts an integer to a byte array
        /// </summary>
        /// <param name="value">The integer to convert</param>
        /// <returns>A byte array representation of the integer</returns>
        public static byte[] ToByteArray(this int value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Converts a string to an integer or null if the conversion fails
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <returns>The integer value or null if the conversion fails</returns>
        public static int? ToIntOrNull(this string s)
        {
            if (!int.TryParse(s, out int result))
            {
                return null;
            }

            return result;
        }
    }
}
