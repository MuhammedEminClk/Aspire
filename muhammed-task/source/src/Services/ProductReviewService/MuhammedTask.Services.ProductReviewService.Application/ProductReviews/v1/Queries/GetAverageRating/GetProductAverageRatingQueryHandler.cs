using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetAverageRating;

internal sealed class GetProductAverageRatingQueryHandler(
    IProductReviewQueryRepository repository) : ICachedQueryHandler<GetProductAverageRatingQuery, ProductAverageRatingViewModel>
{
    public async Task<Result<ProductAverageRatingViewModel>> Handle(GetProductAverageRatingQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        double averageRating = await repository.GetAverageRatingByProductIdAsync(productId, cancellationToken);
        int reviewCount = await repository.GetReviewCountByProductIdAsync(productId, cancellationToken);
        return ProductAverageRatingViewModel.Create(request.ProductId, averageRating, reviewCount);
    }
}
