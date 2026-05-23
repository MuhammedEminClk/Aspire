using FluentValidation;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetAverageRating;

internal sealed class GetProductAverageRatingQueryValidator : AbstractValidator<GetProductAverageRatingQuery>
{
    public GetProductAverageRatingQueryValidator() =>
        RuleFor(x => x.ProductId).NotEmpty();
}
