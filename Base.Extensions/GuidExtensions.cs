namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for Guid type
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Checks if a Guid is empty (equal to Guid.Empty)
        /// </summary>
        /// <param name="guid">The Guid to check</param>
        /// <returns>True if the Guid is empty; otherwise, false</returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Checks if a Guid is not empty (not equal to Guid.Empty)
        /// </summary>
        /// <param name="guid">The Guid to check</param>
        /// <returns>True if the Guid is not empty; otherwise, false</returns>
        public static bool IsNotEmpty(this Guid guid)
        {
            return guid != Guid.Empty;
        }

        /// <summary>
        /// Converts a Guid to a URL-safe base64 string
        /// </summary>
        /// <param name="guid">The Guid to convert</param>
        /// <returns>A URL-safe base64 string representation of the Guid</returns>
        public static string ToUrlSafeString(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "");
        }

        /// <summary>
        /// Attempts to parse a string to a Guid
        /// </summary>
        /// <param name="input">The string to parse</param>
        /// <param name="result">When this method returns, contains the parsed Guid if successful; otherwise, Guid.Empty</param>
        /// <returns>True if the string was successfully parsed; otherwise, false</returns>
        public static bool TryParseGuid(this string input, out Guid result)
        {
            return Guid.TryParse(input, out result);
        }

        /// <summary>
        /// Converts a string to a Guid, returning Guid.Empty if parsing fails
        /// </summary>
        /// <param name="input">The string to convert</param>
        /// <returns>The parsed Guid if successful; otherwise, Guid.Empty</returns>
        public static Guid ToGuid(this string input)
        {
            return Guid.TryParse(input, out var result) ? result : Guid.Empty;
        }

        /// <summary>
        /// Creates a sequential Guid that can be used as a database key
        /// </summary>
        /// <returns>A new sequential Guid</returns>
        public static Guid NewSequentialGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.UtcNow;
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Copy(daysArray, 0, guidArray, 10, 2);
            Array.Copy(msecsArray, 2, guidArray, 12, 4);

            return new Guid(guidArray);
        }

        /// <summary>
        /// Converts a nullable Guid to its string representation or empty string if null
        /// </summary>
        /// <param name="guid">The nullable Guid to convert</param>
        /// <returns>The string representation of the Guid or empty string if null</returns>
        public static string ToStringOrEmpty(this Guid? guid)
        {
            return guid?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Validates that a guid is not null or empty
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid? guid) => guid == null || guid == Guid.Empty;

        /// <summary>
        /// Converts a string to a Guid, returning null if parsing fails
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Guid? ToGuidOrNull(this string s)
        {
            if (!Guid.TryParse(s, out Guid result))
            {
                return null;
            }

            return result;
        }

        public static Guid ToGuidOrEmpty(this string s)
        {
            if (!Guid.TryParse(s, out Guid result))
            {
                return Guid.Empty;
            }

            return result;
        }
    }
}