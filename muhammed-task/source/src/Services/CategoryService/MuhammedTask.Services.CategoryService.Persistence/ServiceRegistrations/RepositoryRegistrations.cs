using MuhammedTask.Services.CategoryService.Domain.Categories.Repositories;
using MuhammedTask.Services.CategoryService.Persistence.EntityFrameworkCore.Repositories.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.CategoryService.Persistence.ServiceRegistrations;
internal static class RepositoryRegistrations
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services) =>
        services
            .AddScoped<ICategoryCommandRepository, EfCategoryCommandRepository>()
            .AddScoped<ICategoryQueryRepository, EfCategoryQueryRepository>();
}
