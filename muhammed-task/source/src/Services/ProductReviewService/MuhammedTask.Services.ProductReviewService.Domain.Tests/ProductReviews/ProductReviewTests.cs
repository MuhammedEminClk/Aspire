using CSharpEssentials;
using FluentAssertions;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

namespace MuhammedTask.Services.ProductReviewService.Domain.Tests.ProductReviews;

public sealed class ProductReviewTests
{
    private static ProductReviewCreateParameters ValidCreateParameters(
        ProductId? productId = null,
        UserId? userId = null,
        int? rating = 4,
        string? comment = "Good product") =>
        new(
            productId ?? ProductId.From(Guid.NewGuid()),
            userId ?? UserId.From(Guid.NewGuid()),
            rating,
            comment);

    [Fact]
    public void Create_WithValidParameters_Succeeds()
    {
        Result<ProductReview> result = ProductReview.Create(ValidCreateParameters());

        result.IsSuccess.Should().BeTrue();
        result.Value.Rating.Value.Should().Be(4);
        result.Value.Comment!.Value.Value.Should().Be("Good product");
    }

    [Fact]
    public void Create_WithNullComment_Succeeds()
    {
        Result<ProductReview> result = ProductReview.Create(ValidCreateParameters(comment: null));

        result.IsSuccess.Should().BeTrue();
        result.Value.Comment!.Value.Value.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyProductId_ReturnsFailure()
    {
        Result<ProductReview> result = ProductReview.Create(ValidCreateParameters(productId: ProductId.Empty));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyUserId_ReturnsFailure()
    {
        Result<ProductReview> result = ProductReview.Create(ValidCreateParameters(userId: UserId.Empty));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_WithInvalidRating_ReturnsFailure()
    {
        Result<ProductReview> result = ProductReview.Create(ValidCreateParameters(rating: 6));

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == "ProductReview.Rating.OutOfRange");
    }

    [Fact]
    public void Create_RaisesProductReviewCreatedDomainEvent()
    {
        Result<ProductReview> result = ProductReview.Create(ValidCreateParameters());

        result.IsSuccess.Should().BeTrue();
        result.Value.DomainEvents.Should().ContainSingle(e =>
            e.GetType().Name == "ProductReviewCreatedDomainEvent");
    }

    [Fact]
    public void Update_ByOwner_Succeeds()
    {
        var ownerId = UserId.From(Guid.NewGuid());
        ProductReview review = ProductReview.Create(ValidCreateParameters(userId: ownerId)).Value;

        var updateParams = new ProductReviewUpdateParameters(review.Id, ownerId, 5, "Updated comment");
        Result result = review.Update(updateParams);

        result.IsSuccess.Should().BeTrue();
        review.Rating.Value.Should().Be(5);
        review.Comment!.Value.Value.Should().Be("Updated comment");
    }

    [Fact]
    public void Update_ByNonOwner_ReturnsUnauthorized()
    {
        ProductReview review = ProductReview.Create(ValidCreateParameters()).Value;

        ProductReviewUpdateParameters updateParams = new(review.Id, UserId.From(Guid.NewGuid()), 5, null);
        Result result = review.Update(updateParams);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == "ProductReview.Unauthorized");
    }

    [Fact]
    public void Delete_ByOwner_Succeeds()
    {
        var ownerId = UserId.From(Guid.NewGuid());
        ProductReview review = ProductReview.Create(ValidCreateParameters(userId: ownerId)).Value;
        review.ClearDomainEvents();

        Result result = review.Delete(ownerId);

        result.IsSuccess.Should().BeTrue();
        review.DomainEvents.Should().ContainSingle(e =>
            e.GetType().Name == "ProductReviewDeletedDomainEvent");
    }

    [Fact]
    public void Delete_ByNonOwner_ReturnsUnauthorized()
    {
        ProductReview review = ProductReview.Create(ValidCreateParameters()).Value;

        Result result = review.Delete(UserId.From(Guid.NewGuid()));

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == "ProductReview.Unauthorized");
    }
}
