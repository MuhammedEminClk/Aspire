using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetList;

internal sealed class GetProductReviewListQueryHandler(
    IProductReviewQueryRepository repository) : ICachedQueryHandler<GetProductReviewListQuery, ProductReviewViewModel[]>
{
    public async Task<Result<ProductReviewViewModel[]>> Handle(GetProductReviewListQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        ProductReviewReadModel[] reviews = await repository.GetProductReviewsByProductIdAsync(productId, cancellationToken);
        return ProductReviewViewModel.Create(reviews);
    }
}
