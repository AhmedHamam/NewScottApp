using System.Text.Json;
using System.Text.Json.Serialization;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for JSON operations
    /// </summary>
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        /// <summary>
        /// Serializes an object to a JSON string with optional custom settings
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="obj">The object to serialize</param>
        /// <param name="options">Optional JSON serializer options</param>
        /// <returns>A JSON string representation of the object</returns>
        public static string ToJson<T>(this T obj, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(obj, options ?? DefaultOptions);
        }

        /// <summary>
        /// Deserializes a JSON string to an object of type T
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="json">The JSON string</param>
        /// <param name="options">Optional JSON serializer options</param>
        /// <returns>The deserialized object</returns>
        public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<T>(json, options ?? DefaultOptions);
        }

        /// <summary>
        /// Attempts to deserialize a JSON string to an object of type T
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="json">The JSON string</param>
        /// <param name="result">The deserialized object if successful</param>
        /// <param name="options">Optional JSON serializer options</param>
        /// <returns>True if deserialization was successful; otherwise, false</returns>
        public static bool TryFromJson<T>(this string json, out T? result, JsonSerializerOptions? options = null)
        {
            try
            {
                result = json.FromJson<T>(options);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Converts a JSON string to a dynamic object
        /// </summary>
        /// <param name="json">The JSON string</param>
        /// <returns>A dynamic object representing the JSON</returns>
        public static dynamic? ToDynamic(this string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ExpandoObject>(json, DefaultOptions);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a JSON string to a dictionary
        /// </summary>
        /// <param name="json">The JSON string</param>
        /// <returns>A dictionary representing the JSON object</returns>
        public static Dictionary<string, object?>? ToDictionary(this string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object?>>(json, DefaultOptions);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if a string is valid JSON
        /// </summary>
        /// <param name="strInput">The string to validate</param>
        /// <returns>True if the string is valid JSON; otherwise, false</returns>
        public static bool IsValidJson(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return false;
            try
            {
                JsonDocument.Parse(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value from a JSON string using a JSON path
        /// </summary>
        /// <typeparam name="T">The type of value to return</typeparam>
        /// <param name="json">The JSON string</param>
        /// <param name="path">The JSON path (e.g., "person.address.city")</param>
        /// <returns>The value at the specified path</returns>
        public static T? GetValueFromPath<T>(this string json, string path)
        {
            try
            {
                var jsonNode = JsonNode.Parse(json);
                if (jsonNode == null) return default;

                var pathParts = path.Split('.');
                foreach (var part in pathParts)
                {
                    jsonNode = jsonNode[part];
                    if (jsonNode == null) return default;
                }

                return jsonNode.GetValue<T>();
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Merges two JSON objects
        /// </summary>
        /// <param name="json1">The first JSON string</param>
        /// <param name="json2">The second JSON string</param>
        /// <returns>A merged JSON string</returns>
        public static string MergeJson(this string json1, string json2)
        {
            try
            {
                var obj1 = JsonNode.Parse(json1)?.AsObject();
                var obj2 = JsonNode.Parse(json2)?.AsObject();

                if (obj1 == null || obj2 == null)
                    throw new ArgumentException("Invalid JSON input");

                foreach (var property in obj2)
                {
                    obj1[property.Key] = property.Value;
                }

                return obj1.ToJsonString(DefaultOptions);
            }
            catch
            {
                return json1;
            }
        }

        /// <summary>
        /// Formats a JSON string with proper indentation
        /// </summary>
        /// <param name="json">The JSON string to format</param>
        /// <returns>A formatted JSON string</returns>
        public static string FormatJson(this string json)
        {
            try
            {
                var jsonObj = JsonSerializer.Deserialize<JsonElement>(json);
                return JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                return json;
            }
        }

        /// <summary>
        /// Compresses a JSON string by removing whitespace
        /// </summary>
        /// <param name="json">The JSON string to compress</param>
        /// <returns>A compressed JSON string</returns>
        public static string CompressJson(this string json)
        {
            try
            {
                var jsonObj = JsonSerializer.Deserialize<JsonElement>(json);
                return JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = false });
            }
            catch
            {
                return json;
            }
        }

        /// <summary>
        /// Flattens a nested JSON object into a dictionary with dot notation keys
        /// </summary>
        /// <param name="json">The JSON string to flatten</param>
        /// <returns>A dictionary with flattened keys</returns>
        public static Dictionary<string, object?> FlattenJson(this string json)
        {
            var result = new Dictionary<string, object?>();
            try
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                FlattenJsonElement(jsonElement, "", result);
            }
            catch { }
            return result;
        }

        private static void FlattenJsonElement(JsonElement element, string prefix, Dictionary<string, object?> result)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        var newPrefix = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                        FlattenJsonElement(property.Value, newPrefix, result);
                    }
                    break;

                case JsonValueKind.Array:
                    var index = 0;
                    foreach (var item in element.EnumerateArray())
                    {
                        var newPrefix = $"{prefix}[{index}]";
                        FlattenJsonElement(item, newPrefix, result);
                        index++;
                    }
                    break;

                default:
                    result[prefix] = GetJsonElementValue(element);
                    break;
            }
        }

        private static object? GetJsonElementValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.GetDecimal(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.GetRawText()
            };
        }
    }
}
