using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;
using MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductReviewService.Persistence.ServiceRegistrations;

internal static class RepositoryRegistrations
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IProductReviewCommandRepository, EfProductReviewCommandRepository>()
            .AddScoped<IProductReviewQueryRepository, EfProductReviewQueryRepository>();
}
