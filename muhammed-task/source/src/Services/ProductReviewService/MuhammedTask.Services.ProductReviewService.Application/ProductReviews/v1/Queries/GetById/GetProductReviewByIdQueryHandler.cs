using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetById;

internal sealed class GetProductReviewByIdQueryHandler(
    IProductReviewQueryRepository repository) : ICachedQueryHandler<GetProductReviewByIdQuery, ProductReviewViewModel>
{
    public async Task<Result<ProductReviewViewModel>> Handle(GetProductReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var productReviewId = ProductReviewId.From(request.ProductReviewId);
        Maybe<ProductReviewReadModel> review = await repository.GetProductReviewByIdAsync(productReviewId, cancellationToken);

        return review.Match<Result<ProductReviewViewModel>>(
            value => ProductReviewViewModel.Create(value),
            () => ProductReviewErrors.NotFoundError(productReviewId));
    }
}
