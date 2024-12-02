using Base.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Base.Application.Security;

/// <summary>
/// Extension methods for registering action validators with dependency injection
/// </summary>
public static class ActionValidatorsExtensions
{
    /// <summary>
    /// Registers all action validators from the specified assembly with the service collection
    /// </summary>
    /// <param name="services">The service collection to add the validators to</param>
    /// <param name="assembly">The assembly to scan for validators</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddActionValidatorsFromAssembly(this IServiceCollection services,
        Assembly assembly)
    {
        var validatorTypes = assembly.GetTypesThatInheritsFromAnInterface(typeof(IActionValidator<,>));
        
        foreach (var validatorType in validatorTypes)
        {
            RegisterValidator(services, validatorType);
        }

        return services;
    }

    /// <summary>
    /// Registers all action validators from multiple assemblies with the service collection
    /// </summary>
    /// <param name="services">The service collection to add the validators to</param>
    /// <param name="assemblies">The assemblies to scan for validators</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddActionValidatorsFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            services.AddActionValidatorsFromAssembly(assembly);
        }

        return services;
    }

    /// <summary>
    /// Registers a specific action validator type with the service collection
    /// </summary>
    /// <param name="services">The service collection to add the validator to</param>
    /// <param name="validatorType">The type of validator to register</param>
    private static void RegisterValidator(IServiceCollection services, Type validatorType)
    {
        var interfaceTypes = validatorType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionValidator<,>));

        foreach (var interfaceType in interfaceTypes)
        {
            services.AddTransient(interfaceType, validatorType);
        }
    }

    /// <summary>
    /// Registers all action validators from the assembly containing the specified type
    /// </summary>
    /// <typeparam name="T">The type whose assembly should be scanned for validators</typeparam>
    /// <param name="services">The service collection to add the validators to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddActionValidatorsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        return services.AddActionValidatorsFromAssembly(typeof(T).Assembly);
    }
}