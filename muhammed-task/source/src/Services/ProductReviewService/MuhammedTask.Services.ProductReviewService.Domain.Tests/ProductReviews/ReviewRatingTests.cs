using CSharpEssentials;
using FluentAssertions;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Domain.Tests.ProductReviews;

public sealed class ReviewRatingTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Create_WithValidValue_ReturnsRating(int value)
    {
        Result<ReviewRating> result = ReviewRating.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithNull_ReturnsFailure()
    {
        Result<ReviewRating> result = ReviewRating.Create(null);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == "ProductReview.Rating.Empty");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Create_WithOutOfRangeValue_ReturnsFailure(int value)
    {
        Result<ReviewRating> result = ReviewRating.Create(value);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Code == "ProductReview.Rating.OutOfRange");
    }
}
