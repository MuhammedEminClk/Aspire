using MuhammedTask.BuildingBlocks.Caching.Redis;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL;
using MuhammedTask.BuildingBlocks.OpenTelemetry.Base;

namespace MuhammedTask.Services.ProductService.WebApi.ServiceRegistrations;

internal static class OpenTelemetryDependencyInjection
{
    internal static IServiceCollection ConfigureTelemetries(
        this IServiceCollection services,
        IHostEnvironment hostEnvironment)
    {
        services
           .ConfigureOpenTelemetry(hostEnvironment.ApplicationName)
           .ConfigurePostgresSqlTelemetry()
           .ConfigureCacheServiceTelemetry()
           .AddOtlpExporter();
        return services;
    }
}
