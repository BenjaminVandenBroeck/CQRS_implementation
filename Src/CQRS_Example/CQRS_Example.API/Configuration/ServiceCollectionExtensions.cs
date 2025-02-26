using CQRS_Example.API.Authentication;
using CQRS_Example.Common;
using CQRS_Example.Common.CQRS;
using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.ReadStore;
using CQRS_Example.Database;
using CQRS_Example.Domain.CommandHandlers;
using System.Reflection;

namespace CQRS_Example.API.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDispatcher, Dispatcher>();

            var domainAssembly = Assembly.GetAssembly(typeof(RegisterEmployeeCommandHandler));

            if(domainAssembly== null)
            {
                throw new Exception("Domain assembly not found, check your references");
            }

            services.Scan(scan =>
            {

                var serviceHandler= scan.FromAssemblies(domainAssembly);
                serviceHandler
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>))).AsSelfWithInterfaces().WithTransientLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>))).AsSelfWithInterfaces().WithTransientLifetime()
;            });

            services.Configure<CosmosOptions>(configuration.GetSection("COSMOSDB_SETTINGS"));
            services.AddHttpContextAccessor();
            services.AddTransient<IInfoService, UserInfoService>();
            services.AddTransient<ICosmosService, CosmosService>();
            services.AddTransient(typeof(IEventStore<>), typeof(CosmosEventStore<>));
            services.AddTransient(typeof(IEventStoreRepository<,>), typeof(EventStoreRepository<,>));
            services.AddTransient(typeof(IReadStoreRepository<>), typeof(CosmosReadStoreRepository<>));

            return services;
        }
    }
}
