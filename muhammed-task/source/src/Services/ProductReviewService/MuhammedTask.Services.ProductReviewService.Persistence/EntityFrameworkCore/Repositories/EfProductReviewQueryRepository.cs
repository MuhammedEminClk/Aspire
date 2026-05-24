using CSharpEssentials;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;
using MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Repositories;

internal sealed class EfProductReviewQueryRepository(
    ApplicationReadDbContext context) : IProductReviewQueryRepository
{
    public async Task<Maybe<ProductReviewReadModel>> GetProductReviewByIdAsync(ProductReviewId productReviewId, CancellationToken cancellationToken = default)
    {
        return await context.ProductReviews
            .Where(r => r.Id == productReviewId.Value)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(ProductReviewReadModel[] Items, int TotalCount)> GetProductReviewsByProductIdAsync(ProductId productId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        IOrderedQueryable<ProductReviewReadModel> query = context.ProductReviews
            .Where(r => r.ProductId == productId.Value)
            .OrderByDescending(r => r.CreatedAt);

        int totalCount = await query.CountAsync(cancellationToken);
        ProductReviewReadModel[] items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<double> GetAverageRatingByProductIdAsync(ProductId productId, CancellationToken cancellationToken = default)
    {
        double? average = await context.ProductReviews
            .Where(r => r.ProductId == productId.Value)
            .AverageAsync(r => (double?)r.Rating, cancellationToken);

        return average ?? 0.0;
    }

    public Task<int> GetReviewCountByProductIdAsync(ProductId productId, CancellationToken cancellationToken = default)
    {
        return context.ProductReviews
            .Where(r => r.ProductId == productId.Value)
            .CountAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(ProductId productId, UserId userId, CancellationToken cancellationToken = default)
    {
        return context.ProductReviews
            .AnyAsync(r => r.ProductId == productId.Value && r.UserId == userId.Value, cancellationToken);
    }
}
