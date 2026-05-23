namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;

public readonly record struct ProductAverageRatingViewModel
{
    private ProductAverageRatingViewModel(Guid productId, double averageRating, int reviewCount)
    {
        ProductId = productId;
        AverageRating = averageRating;
        ReviewCount = reviewCount;
    }

    public readonly Guid ProductId { get; init; }
    public readonly double AverageRating { get; init; }
    public readonly int ReviewCount { get; init; }

    public static ProductAverageRatingViewModel Create(Guid productId, double averageRating, int reviewCount) =>
        new(productId, averageRating, reviewCount);
}
