using Base.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Application;

/// <summary>
/// Extension methods for configuring application layer services in the dependency injection container
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds application layer services to the dependency injection container
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <returns>The IServiceCollection for chaining</returns>
    /// <remarks>
    /// This method registers the following pipeline behaviors in order:
    /// 1. UnhandledExceptionBehaviour - Catches and handles unhandled exceptions
    /// 2. ActionValidationBehaviour - Validates action-specific rules
    /// 3. ValidationBehaviour - Validates request objects using FluentValidation
    /// 4. PerformanceBehaviour - Monitors and logs slow-running requests
    /// 5. CachingBehaviour - Implements response caching
    /// 6. ResetCacheBehaviour - Handles cache invalidation
    /// </remarks>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR pipeline behaviors in specific order
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ActionValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResetCacheBehaviour<,>));

        return services;
    }
}