using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetById;

public sealed record GetProductReviewByIdQuery(Guid ProductReviewId) : ICachedQuery<ProductReviewViewModel>
{
    public bool BypassCache => false;
    public bool CacheFailures => true;
    public string CacheKey => $"productreview:{ProductReviewId}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    public string[] Tags => [];
}
