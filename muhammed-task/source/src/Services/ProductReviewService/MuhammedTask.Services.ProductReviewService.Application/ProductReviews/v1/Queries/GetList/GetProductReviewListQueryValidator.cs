using FluentValidation;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetList;

internal sealed class GetProductReviewListQueryValidator : AbstractValidator<GetProductReviewListQuery>
{
    public GetProductReviewListQueryValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
