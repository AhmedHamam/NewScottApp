using Configuration.Reprisotry.QueriesRepositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Configuration.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IAlertClassificationRepository, AlertClassificationRepository>();
            return services;
        }
    }
}
