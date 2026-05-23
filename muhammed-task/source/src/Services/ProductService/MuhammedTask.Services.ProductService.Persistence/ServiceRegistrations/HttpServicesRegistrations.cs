using MuhammedTask.BuildingBlocks.Persistence.Extensions;
using MuhammedTask.BuildingBlocks.Persistence.HttpHandlers;
using MuhammedTask.Services.Info;
using MuhammedTask.Services.ProductService.Persistence.Categories.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace MuhammedTask.Services.ProductService.Persistence.ServiceRegistrations;
internal static class HttpServicesRegistrations
{
    public static IServiceCollection RegisterHttpServices(this IServiceCollection services)
    {
        services.AddTransient<AuthHeaderHandler>();
        services
            .AddRefitClient<IHttpCategoryService>()
            .ConfigureHttpClientWithServiceName(ServiceKeys.CategoryService)
            .AddHttpMessageHandler<AuthHeaderHandler>();


        return services;
    }
}
