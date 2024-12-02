using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for enum types
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the description attribute value of an enum value
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <returns>The description attribute value if present; otherwise, the enum value's name</returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// Gets the display name of an enum value
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <returns>The display name attribute value if present; otherwise, the enum value's name with spaces between words</returns>
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            var attribute = field.GetCustomAttribute<DisplayNameAttribute>();
            if (attribute != null)
                return attribute.DisplayName;

            // Convert PascalCase to spaced words (e.g., "FirstName" to "First Name")
            return string.Concat(value.ToString().Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart();
        }

        /// <summary>
        /// Converts a string to an enum value
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The string value to convert</param>
        /// <param name="ignoreCase">Whether to ignore case when parsing</param>
        /// <returns>The enum value if successful; otherwise, the default value of the enum</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(value)) return default;
            return Enum.TryParse<T>(value, ignoreCase, out var result) ? result : default;
        }

        /// <summary>
        /// Gets all values of an enum type
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>An array containing all values of the enum type</returns>
        public static T[] GetValues<T>() where T : struct, Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Gets all values and descriptions of an enum type as a dictionary
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>A dictionary with enum values as keys and their descriptions as values</returns>
        public static Dictionary<T, string> GetValuesAndDescriptions<T>() where T : struct, Enum
        {
            return GetValues<T>().ToDictionary(value => value, value => value.GetDescription());
        }

        /// <summary>
        /// Checks if an enum value has a specific flag set
        /// </summary>
        /// <param name="value">The enum value to check</param>
        /// <param name="flag">The flag to check for</param>
        /// <returns>True if the flag is set; otherwise, false</returns>
        public static bool HasFlag(this Enum value, Enum flag)
        {
            if (value.GetType() != flag.GetType())
                throw new ArgumentException("Enum types must match", nameof(flag));

            var valueAsLong = Convert.ToInt64(value);
            var flagAsLong = Convert.ToInt64(flag);
            return (valueAsLong & flagAsLong) == flagAsLong;
        }
    }
}