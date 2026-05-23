using CSharpEssentials;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

public interface IProductReviewCommandRepository
{
    Task<Result<ProductReviewId>> CreateProductReviewAsync(ProductReviewCreateParameters parameters, CancellationToken cancellationToken = default);
    Task<Result> UpdateProductReviewAsync(ProductReviewUpdateParameters parameters, CancellationToken cancellationToken = default);
    Task<Result> DeleteProductReviewAsync(ProductReviewId productReviewId, UserId userId, CancellationToken cancellationToken = default);
}
