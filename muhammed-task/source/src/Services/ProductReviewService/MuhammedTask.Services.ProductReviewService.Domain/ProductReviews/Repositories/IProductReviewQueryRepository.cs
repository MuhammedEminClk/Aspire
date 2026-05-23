using CSharpEssentials;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

public interface IProductReviewQueryRepository
{
    Task<Maybe<ProductReviewReadModel>> GetProductReviewByIdAsync(ProductReviewId productReviewId, CancellationToken cancellationToken = default);
    Task<ProductReviewReadModel[]> GetProductReviewsByProductIdAsync(ProductId productId, CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingByProductIdAsync(ProductId productId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountByProductIdAsync(ProductId productId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(ProductId productId, UserId userId, CancellationToken cancellationToken = default);
}
