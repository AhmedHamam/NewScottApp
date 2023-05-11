namespace Base.Extensions;

public static class ListExtension
{
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