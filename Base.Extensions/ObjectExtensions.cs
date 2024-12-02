using System.ComponentModel;
using System.Text.Json;

namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for object operations
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object to a different type using type conversion
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">The object to convert</param>
        /// <returns>The converted value</returns>
        public static T? To<T>(this object? value)
        {
            if (value == null)
                return default;

            if (typeof(T) == value.GetType())
                return (T)value;

            var targetType = typeof(T);

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(targetType)!;
            }

            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(value.GetType()))
                return (T)converter.ConvertFrom(value)!;

            converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter.CanConvertTo(targetType))
                return (T)converter.ConvertTo(value, targetType)!;

            return default;
        }

        /// <summary>
        /// Checks if an object is null
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>True if the object is null; otherwise, false</returns>
        public static bool IsNull(this object? obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Creates a deep clone of an object using JSON serialization
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="obj">The object to clone</param>
        /// <returns>A deep clone of the object</returns>
        public static T? DeepClone<T>(this T obj)
        {
            if (obj == null)
                return default;

            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Checks if an object has a specific property
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>True if the property exists; otherwise, false</returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Gets the value of a property by its name
        /// </summary>
        /// <param name="obj">The object containing the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property</returns>
        public static object? GetPropertyValue(this object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);
            return property?.GetValue(obj);
        }

        /// <summary>
        /// Sets the value of a property by its name
        /// </summary>
        /// <param name="obj">The object containing the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to set</param>
        /// <returns>True if the property was set; otherwise, false</returns>
        public static bool SetPropertyValue(this object obj, string propertyName, object? value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts an object to a dictionary
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A dictionary containing the object's properties and values</returns>
        public static IDictionary<string, object?> ToDictionary(this object obj)
        {
            var dictionary = new Dictionary<string, object?>();
            foreach (var property in obj.GetType().GetProperties())
            {
                dictionary[property.Name] = property.GetValue(obj);
            }
            return dictionary;
        }

        /// <summary>
        /// Checks if all properties of an object are null or default
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>True if all properties are null or default; otherwise, false</returns>
        public static bool IsEmpty(this object obj)
        {
            return obj.GetType().GetProperties()
                .All(p => p.GetValue(obj) == null || p.GetValue(obj)?.Equals(GetDefault(p.PropertyType)) == true);
        }

        private static object? GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Converts an object to its JSON representation
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A JSON string representation of the object</returns>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// Tries to convert an object to a specified type
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="obj">The object to convert</param>
        /// <param name="result">The converted value if successful</param>
        /// <returns>True if the conversion was successful; otherwise, false</returns>
        public static bool TryConvert<T>(this object obj, out T? result)
        {
            try
            {
                result = obj.To<T>();
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
