using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for Dictionary operations
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value from the dictionary with a default if the key doesn't exist
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key to look up</param>
        /// <param name="defaultValue">The default value if key is not found</param>
        /// <returns>The value if found, otherwise the default value</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default!) where TKey : notnull
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }

        /// <summary>
        /// Adds or updates a value in the dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key to add or update</param>
        /// <param name="addValue">The value to add if the key doesn't exist</param>
        /// <param name="updateValueFactory">The function to create the updated value if the key exists</param>
        /// <returns>The new value</returns>
        public static TValue AddOrUpdate<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue addValue,
            Func<TKey, TValue, TValue> updateValueFactory) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                var newValue = updateValueFactory(key, value);
                dictionary[key] = newValue;
                return newValue;
            }

            dictionary[key] = addValue;
            return addValue;
        }

        /// <summary>
        /// Gets or adds a value to the dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key to look up</param>
        /// <param name="valueFactory">The function to create the value if the key doesn't exist</param>
        /// <returns>The existing or new value</returns>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> valueFactory) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            var newValue = valueFactory(key);
            dictionary[key] = newValue;
            return newValue;
        }

        /// <summary>
        /// Removes multiple keys from the dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="keys">The keys to remove</param>
        /// <returns>The number of items removed</returns>
        public static int RemoveRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<TKey> keys) where TKey : notnull
        {
            var count = 0;
            foreach (var key in keys)
            {
                if (dictionary.Remove(key))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Adds a range of key-value pairs to the dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="items">The items to add</param>
        public static void AddRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> items) where TKey : notnull
        {
            foreach (var item in items)
            {
                dictionary[item.Key] = item.Value;
            }
        }

        /// <summary>
        /// Converts a dictionary to a concurrent dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary to convert</param>
        /// <returns>A new concurrent dictionary</returns>
        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary) where TKey : notnull
        {
            return new ConcurrentDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Merges two dictionaries
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The first dictionary</param>
        /// <param name="other">The second dictionary</param>
        /// <param name="replaceExisting">If true, values from other replace existing values</param>
        /// <returns>A new merged dictionary</returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IDictionary<TKey, TValue> other,
            bool replaceExisting = true) where TKey : notnull
        {
            var result = new Dictionary<TKey, TValue>(dictionary);
            foreach (var pair in other)
            {
                if (replaceExisting || !result.ContainsKey(pair.Key))
                {
                    result[pair.Key] = pair.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a value from a nested dictionary using a path
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="path">The path to the value (e.g., "key1.key2.key3")</param>
        /// <returns>The value if found, otherwise default</returns>
        public static TValue? GetNestedValue<TValue>(
            this IDictionary<string, object> dictionary,
            string path)
        {
            var current = dictionary;
            var parts = path.Split('.');

            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (!current.TryGetValue(parts[i], out var next) || !(next is IDictionary<string, object> nextDict))
                    return default;

                current = nextDict;
            }

            if (current.TryGetValue(parts[^1], out var value) && value is TValue typedValue)
                return typedValue;

            return default;
        }

        /// <summary>
        /// Sets a value in a nested dictionary using a path
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="path">The path to the value (e.g., "key1.key2.key3")</param>
        /// <param name="value">The value to set</param>
        public static void SetNestedValue<TValue>(
            this IDictionary<string, object> dictionary,
            string path,
            TValue value)
        {
            var current = dictionary;
            var parts = path.Split('.');

            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (!current.TryGetValue(parts[i], out var next))
                {
                    next = new Dictionary<string, object>();
                    current[parts[i]] = next;
                }

                if (!(next is IDictionary<string, object> nextDict))
                    throw new InvalidOperationException($"Path segment '{parts[i]}' is not a dictionary");

                current = nextDict;
            }

            current[parts[^1]] = value!;
        }

        /// <summary>
        /// Flattens a nested dictionary into a single-level dictionary
        /// </summary>
        /// <param name="dictionary">The dictionary to flatten</param>
        /// <param name="separator">The separator to use in flattened keys</param>
        /// <returns>A flattened dictionary</returns>
        public static Dictionary<string, object> Flatten(
            this IDictionary<string, object> dictionary,
            string separator = ".")
        {
            var result = new Dictionary<string, object>();
            FlattenRecursive(dictionary, "", separator, result);
            return result;
        }

        private static void FlattenRecursive(
            IDictionary<string, object> dictionary,
            string prefix,
            string separator,
            Dictionary<string, object> result)
        {
            foreach (var pair in dictionary)
            {
                var key = string.IsNullOrEmpty(prefix) ? pair.Key : prefix + separator + pair.Key;

                if (pair.Value is IDictionary<string, object> nestedDict)
                {
                    FlattenRecursive(nestedDict, key, separator, result);
                }
                else
                {
                    result[key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Filters a dictionary based on a predicate
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary to filter</param>
        /// <param name="predicate">The predicate to filter with</param>
        /// <returns>A new filtered dictionary</returns>
        public static Dictionary<TKey, TValue> Filter<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> predicate) where TKey : notnull
        {
            return dictionary.Where(predicate).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Maps dictionary values to new values
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the input value</typeparam>
        /// <typeparam name="TResult">The type of the output value</typeparam>
        /// <param name="dictionary">The dictionary to map</param>
        /// <param name="selector">The mapping function</param>
        /// <returns>A new dictionary with mapped values</returns>
        public static Dictionary<TKey, TResult> MapValues<TKey, TValue, TResult>(
            this IDictionary<TKey, TValue> dictionary,
            Func<TValue, TResult> selector) where TKey : notnull
        {
            return dictionary.ToDictionary(x => x.Key, x => selector(x.Value));
        }

        /// <summary>
        /// Converts a dictionary to a string representation
        /// Format: key1=value1;key2=value2;key3=value3
        /// Special characters in keys and values are properly escaped
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="dictionary">The dictionary to convert</param>
        /// <param name="keyValueSeparator">The separator between keys and values (default: '=')</param>
        /// <param name="pairSeparator">The separator between key-value pairs (default: ';')</param>
        /// <returns>A string representation of the dictionary</returns>
        public static string ToString<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            char keyValueSeparator = '=',
            char pairSeparator = ';') where TKey : notnull
        {
            if (dictionary == null || dictionary.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            var isFirst = true;

            foreach (var pair in dictionary)
            {
                if (!isFirst)
                    sb.Append(pairSeparator);

                sb.Append(EscapeString(pair.Key?.ToString(), keyValueSeparator, pairSeparator))
                  .Append(keyValueSeparator)
                  .Append(EscapeString(pair.Value?.ToString(), keyValueSeparator, pairSeparator));

                isFirst = false;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a dictionary from its string representation
        /// Format: key1=value1;key2=value2;key3=value3
        /// Handles escaped special characters in keys and values
        /// </summary>
        /// <param name="str">The string to parse</param>
        /// <param name="keyValueSeparator">The separator between keys and values (default: '=')</param>
        /// <param name="pairSeparator">The separator between key-value pairs (default: ';')</param>
        /// <returns>A dictionary parsed from the string</returns>
        public static Dictionary<string, string> FromString(
            this string str,
            char keyValueSeparator = '=',
            char pairSeparator = ';')
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(str))
                return result;

            var pairs = SplitUnescaped(str, pairSeparator);
            foreach (var pair in pairs)
            {
                var keyValue = SplitUnescaped(pair, keyValueSeparator);
                if (keyValue.Length != 2)
                    continue;

                var key = UnescapeString(keyValue[0].Trim());
                var value = UnescapeString(keyValue[1].Trim());

                if (!string.IsNullOrEmpty(key))
                    result[key] = value;
            }

            return result;
        }

        /// <summary>
        /// Creates a typed dictionary from its string representation
        /// Format: key1=value1;key2=value2;key3=value3
        /// Handles escaped special characters in keys and values
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="str">The string to parse</param>
        /// <param name="keyConverter">Function to convert string to TKey</param>
        /// <param name="valueConverter">Function to convert string to TValue</param>
        /// <param name="keyValueSeparator">The separator between keys and values (default: '=')</param>
        /// <param name="pairSeparator">The separator between key-value pairs (default: ';')</param>
        /// <returns>A typed dictionary parsed from the string</returns>
        public static Dictionary<TKey, TValue> FromString<TKey, TValue>(
            this string str,
            Func<string, TKey> keyConverter,
            Func<string, TValue> valueConverter,
            char keyValueSeparator = '=',
            char pairSeparator = ';') where TKey : notnull
        {
            var result = new Dictionary<TKey, TValue>();
            var stringDict = FromString(str, keyValueSeparator, pairSeparator);

            foreach (var pair in stringDict)
            {
                try
                {
                    var key = keyConverter(pair.Key);
                    var value = valueConverter(pair.Value);
                    result[key] = value;
                }
                catch
                {
                    // Skip entries that can't be converted
                    continue;
                }
            }

            return result;
        }

        private static string EscapeString(string? str, char keyValueSeparator, char pairSeparator)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str.Replace("\\", "\\\\")
                     .Replace(keyValueSeparator.ToString(), "\\" + keyValueSeparator)
                     .Replace(pairSeparator.ToString(), "\\" + pairSeparator);
        }

        private static string UnescapeString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            var isEscaped = false;

            foreach (var c in str)
            {
                if (isEscaped)
                {
                    result.Append(c);
                    isEscaped = false;
                }
                else if (c == '\\')
                {
                    isEscaped = true;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        private static string[] SplitUnescaped(string str, char separator)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            var isEscaped = false;

            foreach (var c in str)
            {
                if (isEscaped)
                {
                    current.Append(c);
                    isEscaped = false;
                }
                else if (c == '\\')
                {
                    isEscaped = true;
                }
                else if (c == separator)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            result.Add(current.ToString());
            return result.ToArray();
        }
    }
}
