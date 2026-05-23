using CSharpEssentials.EntityFrameworkCore.Interceptors;
using MassTransit;
using MuhammedTask.BuildingBlocks.BackgroundJobs.Quartz;
using MuhammedTask.BuildingBlocks.Caching.Redis;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL;
using MuhammedTask.BuildingBlocks.MessageBrokers.MassTransit;
using MuhammedTask.Services.Info;
using MuhammedTask.Services.ProductService.Persistence.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace MuhammedTask.Services.ProductService.Persistence.ServiceRegistrations;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        return services
            .ConfigureCacheServices(environment, configuration)
            .AddQuartzWithHostedService()
            .AddDatabaseServices(configuration)
            .AddEventBus(
                configure => configure.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(configuration.GetConnectionString(ServiceKeys.RabbitMQ));
                        cfg.ConfigureEndpoints(context);
                    }),
                assemblies: PersistenceAssemblyReference.Assembly)
            .RegisterRepositories()
            .RegisterHttpServices()
            .RegisterServices();
    }
    private static IServiceCollection ConfigureCacheServices(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        return services
            .AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(sp =>
            {
                string connectionString = configuration.GetConnectionString(ServiceKeys.Redis);
                var configurationOptions = ConfigurationOptions.Parse(connectionString!, true);
                configurationOptions.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configurationOptions);
            })
            .AddCacheServices(
                environment.ApplicationName,
                redisConfigurations => redisConfigurations.Configuration = configuration.GetConnectionString(ServiceKeys.Redis),
                memoryConfiguration =>
                {
                });
    }
    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string migrationsAssembly = typeof(PersistenceAssemblyReference).Namespace;
        return services
            .AddSingleton<SlowQueryInterceptor>()
            .AddPooledPostgresDbContext<ApplicationWriteDbContext>(
                configuration.GetConnectionString(ServiceKeys.Database.PostgresProductService)!,
                options => options.MigrationsAssembly = migrationsAssembly)
            .AddPooledPostgresDbContext<ApplicationReadDbContext>(
                configuration.GetConnectionString(ServiceKeys.Database.PostgresProductService)!,
                options =>
                {
                    options.MigrationsAssembly = migrationsAssembly;
                    options.EnablePublishDomainEventsInterceptor = false;
                    options.EnableAuditableInterceptor = false;
                    options.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                });
    }
}
