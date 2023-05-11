using Base.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Base.Application.Security;

public static class AuthorizersExtensions
{
    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services,
        Assembly assembly)
    {
        assembly.GetTypesThatInheritsFromAnInterface(typeof(IActionValidator<,>))
            .ForEach(t =>
            {
                var interfaceType = t.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<,>));
                services.AddTransient(interfaceType, t);
            });
        return services;
    }
}