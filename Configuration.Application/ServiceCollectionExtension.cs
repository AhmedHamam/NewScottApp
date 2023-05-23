using Configuration.Infrastructure.Repositories;
using Configuration.Infrastructure.Repositories.QueriesRepositories;
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


            services.AddScoped<ICountryQueryRepository, CountryQueryRepository>();
            services.AddTransient<IAlertClassificationRepository, AlertClassificationRepository>();

            return services;
        }
    }
}
