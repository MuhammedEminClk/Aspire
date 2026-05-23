using CSharpEssentials;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

public readonly record struct ProductReviewId
{
    private ProductReviewId(Guid value) => Value = value;
    public Guid Value { get; }
    public static ProductReviewId New() => new(Guider.NewGuid());
    public static ProductReviewId From(Guid value) => new(value);
    public static readonly ProductReviewId Empty = new(Guid.Empty);
}
