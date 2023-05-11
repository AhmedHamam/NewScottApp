namespace Base.Extensions
{
    public static class IntExtensions
    {
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
