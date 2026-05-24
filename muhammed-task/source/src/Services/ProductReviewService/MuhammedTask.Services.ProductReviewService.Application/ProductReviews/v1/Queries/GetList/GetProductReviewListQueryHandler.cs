using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetList;

internal sealed class GetProductReviewListQueryHandler(
    IProductReviewQueryRepository repository) : ICachedQueryHandler<GetProductReviewListQuery, PaginatedResponse<ProductReviewViewModel>>
{
    public async Task<Result<PaginatedResponse<ProductReviewViewModel>>> Handle(GetProductReviewListQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        (ProductReviewReadModel[] items, int totalCount) = await repository.GetProductReviewsByProductIdAsync(productId, request.PageNumber, request.PageSize, cancellationToken);
        return PaginatedResponse<ProductReviewViewModel>.Create(request.PageNumber, request.PageSize, totalCount, ProductReviewViewModel.Create(items));
    }
}
