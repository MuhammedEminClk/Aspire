using CSharpEssentials;
using CSharpEssentials.Entity;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Events;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;

public sealed class ProductReview : SoftDeletableEntityBase<ProductReviewId>
{
    private ProductReview() { }
    private ProductReview(ProductReviewId id, ProductId productId, UserId userId, ReviewRating rating, ReviewComment? comment)
    {
        Id = id;
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        Raise(new ProductReviewCreatedDomainEvent(id));
    }

    public ProductId ProductId { get; private set; }
    public UserId UserId { get; private set; }
    public ReviewRating Rating { get; private set; }
    public ReviewComment? Comment { get; private set; }

    public static Result<ProductReview> Create(ProductReviewCreateParameters parameters)
    {
        if (parameters.ProductId == ProductId.Empty)
            return ProductReviewErrors.Rating.EmptyError;

        if (parameters.UserId == UserId.Empty)
            return ProductReviewErrors.Rating.EmptyError;

        Result<ReviewRating> rating = ReviewRating.Create(parameters.Rating);
        Result<ReviewComment> comment = ReviewComment.Create(parameters.Comment);

        var result = Result.And(rating, comment);
        if (result.IsFailure)
            return result.Errors;

        return new ProductReview(
            ProductReviewId.New(),
            parameters.ProductId,
            parameters.UserId,
            rating.Value,
            comment.Value);
    }

    public Result Update(ProductReviewUpdateParameters parameters)
    {
        if (UserId != parameters.UserId)
            return ProductReviewErrors.UnauthorizedError;

        Result<ReviewRating> rating = ReviewRating.Create(parameters.Rating);
        Result<ReviewComment> comment = ReviewComment.Create(parameters.Comment);

        var result = Result.And(rating, comment);
        if (result.IsFailure)
            return result.Errors;

        Rating = rating.Value;
        Comment = comment.Value;
        Raise(new ProductReviewUpdatedDomainEvent(Id));

        return Result.Success();
    }

    public Result Delete(UserId userId)
    {
        if (UserId != userId)
            return ProductReviewErrors.UnauthorizedError;

        Raise(new ProductReviewDeletedDomainEvent(Id));
        return Result.Success();
    }
}
