using CSharpEssentials;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

public readonly record struct ReviewRating
{
    public const int MinValue = 1;
    public const int MaxValue = 5;
    public int Value { get; }
    private ReviewRating(int value) => Value = value;
    public static ReviewRating From(int value) => new(value);
    public static Result<ReviewRating> Create(int? value)
    {
        if (value is null)
            return ProductReviewErrors.Rating.EmptyError;

        if (value < MinValue || value > MaxValue)
            return ProductReviewErrors.Rating.OutOfRangeError(value.Value);

        return new ReviewRating(value.Value);
    }
}
