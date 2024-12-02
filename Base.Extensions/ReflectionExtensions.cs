using System.Reflection;
using System.Linq.Expressions;

namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for reflection operations
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the name of a property using a lambda expression
        /// </summary>
        /// <typeparam name="T">The type containing the property</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="expression">The lambda expression selecting the property</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Expression must be a member expression");

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Gets all public properties of a type
        /// </summary>
        /// <param name="type">The type to get properties from</param>
        /// <returns>An array of PropertyInfo objects</returns>
        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Gets all public instance methods of a type
        /// </summary>
        /// <param name="type">The type to get methods from</param>
        /// <returns>An array of MethodInfo objects</returns>
        public static MethodInfo[] GetPublicMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Checks if a type implements a specific interface
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="interfaceType">The interface type to look for</param>
        /// <returns>True if the type implements the interface; otherwise, false</returns>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }

        /// <summary>
        /// Gets all attributes of a specific type from a member
        /// </summary>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <param name="member">The member to get attributes from</param>
        /// <param name="inherit">Whether to search the inheritance chain</param>
        /// <returns>An array of attributes of the specified type</returns>
        public static T[] GetAttributes<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }

        /// <summary>
        /// Gets the first attribute of a specific type from a member
        /// </summary>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <param name="member">The member to get the attribute from</param>
        /// <param name="inherit">Whether to search the inheritance chain</param>
        /// <returns>The first attribute of the specified type, or null if not found</returns>
        public static T? GetAttribute<T>(this MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetAttributes<T>(inherit).FirstOrDefault();
        }

        /// <summary>
        /// Checks if a type has a specific attribute
        /// </summary>
        /// <typeparam name="T">The type of attribute to check for</typeparam>
        /// <param name="type">The type to check</param>
        /// <param name="inherit">Whether to search the inheritance chain</param>
        /// <returns>True if the type has the attribute; otherwise, false</returns>
        public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), inherit).Any();
        }

        /// <summary>
        /// Creates an instance of a type using its parameterless constructor
        /// </summary>
        /// <param name="type">The type to create an instance of</param>
        /// <returns>A new instance of the specified type</returns>
        public static object? CreateInstance(this Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Gets the default value of a type
        /// </summary>
        /// <param name="type">The type to get the default value for</param>
        /// <returns>The default value of the type</returns>
        public static object? GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Gets all types that inherit from a specific interface
        /// </summary>
        /// <param name="assembly">The assembly to search for types</param>
        /// <param name="interfaceType">The interface type to look for</param>
        /// <returns>A list of types that inherit from the interface</returns>
        public static List<Type> GetTypesThatInheritsFromAnInterface(this Assembly assembly, Type interfaceType)
        {
            Type interfaceType2 = interfaceType;
            return (from t in assembly.GetTypes()
                    where t.GetInterfaces()
                        .Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType2)
                    select t).ToList();
        }
    }
}