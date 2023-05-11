namespace Base.Extensions;

public static class GuidExtensions
{
    /// <summary>
    /// Validates that a guid is not null or empty
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this Guid? guid) => guid == null || guid == Guid.Empty;

    /// <summary>
    /// Validates that a guid is not null or empty
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static bool IsEmpty(this Guid guid)
    {
        return guid == Guid.Empty;
    }

    public static Guid ToGuidOrEmpty(this string s)
    {
        if (!Guid.TryParse(s, out Guid result))
        {
            return Guid.Empty;
        }

        return result;
    }

    public static Guid? ToGuidOrNull(this string s)
    {
        if (!Guid.TryParse(s, out Guid result))
        {
            return null;
        }

        return result;
    }
}