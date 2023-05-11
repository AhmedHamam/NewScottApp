using System.Reflection;

namespace Base.Extensions;

public static class ReflectionExtensions
{
    public static List<Type> GetTypesThatInheritsFromAnInterface(this Assembly assembly, Type interfaceType)
    {
        Type interfaceType2 = interfaceType;
        return (from t in assembly.GetTypes()
                where t.GetInterfaces()
                    .Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType2)
                select t).ToList();
    }
}