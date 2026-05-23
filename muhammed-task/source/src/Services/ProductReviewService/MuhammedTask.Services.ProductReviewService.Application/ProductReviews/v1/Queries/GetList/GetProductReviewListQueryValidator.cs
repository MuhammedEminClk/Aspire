using FluentValidation;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetList;

internal sealed class GetProductReviewListQueryValidator : AbstractValidator<GetProductReviewListQuery>
{
    public GetProductReviewListQueryValidator() =>
        RuleFor(x => x.ProductId).NotEmpty();
}
