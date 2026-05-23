using CSharpEssentials;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;

public static class ProductReviewErrors
{
    public static Error NotFoundError(ProductReviewId id) =>
        Error.NotFound(code: "ProductReview.NotFound", description: $"Product review does not exist: {id.Value}");

    public static readonly Error AlreadyReviewedError =
        Error.Conflict(code: "ProductReview.AlreadyReviewed", description: "User has already reviewed this product");

    public static readonly Error UnauthorizedError =
        Error.Forbidden(code: "ProductReview.Unauthorized", description: "User is not authorized to modify this review");

    public static class Rating
    {
        public static readonly Error EmptyError =
            Error.Validation(code: "ProductReview.Rating.Empty", description: "Rating is required");

        public static Error OutOfRangeError(int value) =>
            Error.Validation(code: "ProductReview.Rating.OutOfRange", description: $"Rating must be between {ReviewRating.MinValue} and {ReviewRating.MaxValue}, got: {value}");
    }

    public static class Comment
    {
        public static Error TooLongError(int length) =>
            Error.Validation(code: "ProductReview.Comment.TooLong", description: $"Comment must be at most {ReviewComment.MaxLength} characters, got: {length}");
    }
}
