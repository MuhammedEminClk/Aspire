using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;

public readonly record struct ProductReviewViewModel
{
    private ProductReviewViewModel(ProductReviewReadModel r)
    {
        Id = r.Id;
        ProductId = r.ProductId;
        UserId = r.UserId;
        Rating = r.Rating;
        Comment = r.Comment;
        CreatedAt = r.CreatedAt;
        UpdatedAt = r.UpdatedAt;
    }

    public readonly Guid Id { get; init; }
    public readonly Guid ProductId { get; init; }
    public readonly Guid UserId { get; init; }
    public readonly int Rating { get; init; }
    public readonly string? Comment { get; init; }
    public readonly DateTimeOffset CreatedAt { get; init; }
    public readonly DateTimeOffset? UpdatedAt { get; init; }

    public static ProductReviewViewModel Create(ProductReviewReadModel r) => new(r);
    public static ProductReviewViewModel[] Create(ProductReviewReadModel[] rs) => [.. rs.Select(Create)];
}
