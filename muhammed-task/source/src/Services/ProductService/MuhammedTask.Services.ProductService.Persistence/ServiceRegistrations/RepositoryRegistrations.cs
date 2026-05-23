using MuhammedTask.Services.ProductService.Domain.Categories.Repositories;
using MuhammedTask.Services.ProductService.Domain.Products.Repositories;
using MuhammedTask.Services.ProductService.Persistence.EntityFrameworkCore.Repositories.Categories;
using MuhammedTask.Services.ProductService.Persistence.EntityFrameworkCore.Repositories.Products;

using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductService.Persistence.ServiceRegistrations;
internal static class RepositoryRegistrations
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services) =>
        services
            .AddScoped<ICategoryCommandRepository, EfCategoryCommandRepository>()
            .AddScoped<IProductCommandRepository, EfProductCommandRepository>()
            .AddScoped<IProductQueryRepository, EfProductQueryRepository>();
}
