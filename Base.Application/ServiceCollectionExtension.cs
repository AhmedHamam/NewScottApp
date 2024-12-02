using Base.Application.Behaviors;
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
    /// 1. UnhandledExceptionBehavior - Catches and handles unhandled exceptions
    /// 2. ActionValidationBehavior - Validates action-specific rules
    /// 3. ValidationBehavior - Validates request objects using FluentValidation
    /// 4. PerformanceBehavior - Monitors and logs slow-running requests
    /// 5. CachingBehavior - Implements response caching
    /// 6. ResetCacheBehavior - Handles cache invalidation
    /// </remarks>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR pipeline behaviors in specific order
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ActionValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResetCacheBehavior<,>));

        return services;
    }
}