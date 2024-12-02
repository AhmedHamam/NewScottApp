namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for IEnumerable and List types
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Checks if a collection is null or empty
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <returns>True if the collection is null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Checks if a collection has any elements (is not null or empty)
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The collection to check</param>
        /// <returns>True if the collection has any elements; otherwise, false</returns>
        public static bool HasItems<T>(this IEnumerable<T>? source)
        {
            return !source.IsNullOrEmpty();
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified property selector
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the property to distinguish elements by</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <returns>An IEnumerable that contains distinct elements from the source sequence</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Adds a range of items to a list if they are not already present
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The list to add items to</param>
        /// <param name="items">The items to add</param>
        public static void AddRangeIfNotExists<T>(this List<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!source.Contains(item))
                {
                    source.Add(item);
                }
            }
        }

        /// <summary>
        /// Returns a random element from a sequence
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence</typeparam>
        /// <param name="source">The sequence to get a random element from</param>
        /// <returns>A random element from the sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown when source is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when source is empty</exception>
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var list = source.ToList();
            if (list.Count == 0)
                throw new InvalidOperationException("The source sequence is empty");

            return list[Random.Shared.Next(list.Count)];
        }

        /// <summary>
        /// Splits a sequence into chunks of a specified size
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence</typeparam>
        /// <param name="source">The sequence to split into chunks</param>
        /// <param name="chunkSize">The size of each chunk</param>
        /// <returns>An IEnumerable of chunks, where each chunk is an IEnumerable of the source type</returns>
        /// <exception cref="ArgumentNullException">Thrown when source is null</exception>
        /// <exception cref="ArgumentException">Thrown when chunkSize is less than 1</exception>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (chunkSize < 1)
                throw new ArgumentException("Chunk size must be greater than 0", nameof(chunkSize));

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value));
        }

        /// <summary>
        /// Returns the index of the first element in the sequence that satisfies a condition
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence</typeparam>
        /// <param name="source">The sequence to search</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>The zero-based index of the first element that satisfies the condition, or -1 if no element satisfies the condition</returns>
        public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int index = 0;
            foreach (T item in source)
            {
                if (predicate(item))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Performs an action on each element of a sequence
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence</typeparam>
        /// <param name="source">The sequence to perform the action on</param>
        /// <param name="action">The action to perform on each element</param>
        /// <returns>The source sequence</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Get a list as an empty list if equals null
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T>? original)
        {
            return original ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> WhereNotIn<T>(this IEnumerable<T>? original, IEnumerable<T>? query)
        {
            if (original == null || query == null) return Enumerable.Empty<T>();
            return original.Except(query);
        }

        /// <summary>
        /// Checks if a list is empty
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T>? original)
        {
            return original == null || !original.Any();
        }

        /// <summary>
        /// Validates that list has unique values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasUniqueValues<T>(this IEnumerable<T>? list)
            => list.AsNotNull().Distinct().Count() == list.AsNotNull().Count();

        /// <summary>
        /// Validates that a value occurs in a list only ones
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasOneAndUnique<T>(this IEnumerable<T>? list, T value)
            => list.AsNotNull().Count(v => Equals(v, value)) == 1;
    }
}