using CSharpEssentials;
using FluentAssertions;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Domain.Tests.ProductReviews;

public sealed class ReviewCommentTests
{
    [Fact]
    public void Create_WithNull_Succeeds()
    {
        Result<ReviewComment> result = ReviewComment.Create(null);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().BeNull();
    }

    [Fact]
    public void Create_WithValidText_Succeeds()
    {
        Result<ReviewComment> result = ReviewComment.Create("Great product!");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Great product!");
    }

    [Fact]
    public void Create_WithTooLongText_ReturnsFailure()
    {
        string longComment = new('x', ReviewComment.MaxLength + 1);

        Result<ReviewComment> result = ReviewComment.Create(longComment);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == "ProductReview.Comment.TooLong");
    }

    [Fact]
    public void Create_WithExactMaxLength_Succeeds()
    {
        string comment = new('x', ReviewComment.MaxLength);

        Result<ReviewComment> result = ReviewComment.Create(comment);

        result.IsSuccess.Should().BeTrue();
    }
}
