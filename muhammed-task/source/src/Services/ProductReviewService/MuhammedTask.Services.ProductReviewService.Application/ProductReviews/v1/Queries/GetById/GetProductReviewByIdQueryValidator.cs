using FluentValidation;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetById;

internal sealed class GetProductReviewByIdQueryValidator : AbstractValidator<GetProductReviewByIdQuery>
{
    public GetProductReviewByIdQueryValidator() =>
        RuleFor(x => x.ProductReviewId).NotEmpty();
}
