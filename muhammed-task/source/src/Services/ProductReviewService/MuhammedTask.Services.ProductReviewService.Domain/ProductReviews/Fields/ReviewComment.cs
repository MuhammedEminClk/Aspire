using CSharpEssentials;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

public readonly record struct ReviewComment
{
    public const int MaxLength = 1000;
    public string? Value { get; }
    private ReviewComment(string? value) => Value = value;
    public static ReviewComment From(string? value) => new(value);
    public static Result<ReviewComment> Create(string? value)
    {
        if (value is not null && value.Length > MaxLength)
            return ProductReviewErrors.Comment.TooLongError(value.Length);

        return new ReviewComment(value);
    }
}
