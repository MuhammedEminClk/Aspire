using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetAverageRating;

public sealed record GetProductAverageRatingQuery(Guid ProductId) : ICachedQuery<ProductAverageRatingViewModel>
{
    public bool BypassCache => false;
    public bool CacheFailures => true;
    public string CacheKey => $"product:{ProductId}:average-rating";
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    public string[] Tags => [$"product:{ProductId}", "average-rating"];
}
