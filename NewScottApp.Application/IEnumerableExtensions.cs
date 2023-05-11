using Microsoft.AspNetCore.Identity;
using System.Text;

namespace NewScottApp.Application
{

    public static class IEnumerableExtensions
    {
        public static string ToStringDetails<T>(this IEnumerable<T> enumerable)
        {
            var result = new StringBuilder();
            foreach (var item in enumerable)
            {
                if (item is IdentityError)
                {
                    var error = item as IdentityError;
                    result.Append(error.Code);
                    result.Append(" ");
                    result.Append(error.Description);
                    result.Append(" ");
                }
            }

            return result.ToString();
        }
    }
}
