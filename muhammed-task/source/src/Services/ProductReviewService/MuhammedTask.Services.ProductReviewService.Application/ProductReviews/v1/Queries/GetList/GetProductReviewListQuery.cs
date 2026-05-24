using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetList;

public sealed record GetProductReviewListQuery(Guid ProductId, int PageNumber = 1, int PageSize = 10) : ICachedQuery<PaginatedResponse<ProductReviewViewModel>>
{
    public bool BypassCache => false;
    public bool CacheFailures => true;
    public string CacheKey => $"product:{ProductId}:reviews:page:{PageNumber}:size:{PageSize}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    public string[] Tags => [$"product:{ProductId}", "reviews"];
}
