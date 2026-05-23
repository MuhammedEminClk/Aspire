using MuhammedTask.Services.ProductService.Domain.Categories.Services;
using MuhammedTask.Services.ProductService.Persistence.Categories.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductService.Persistence.ServiceRegistrations;

internal static class ServicesRegistrations
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
