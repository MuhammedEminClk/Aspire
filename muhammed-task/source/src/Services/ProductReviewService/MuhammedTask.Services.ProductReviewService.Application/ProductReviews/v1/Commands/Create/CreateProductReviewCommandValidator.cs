using FluentValidation;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Create;

internal sealed class CreateProductReviewCommandValidator : AbstractValidator<CreateProductReviewCommand>
{
    public CreateProductReviewCommandValidator()
    {
        RuleFor(x => x.Product).NotEmpty();
        RuleFor(x => x.User).NotEmpty();
        RuleFor(x => x.Rating)
            .NotNull()
            .InclusiveBetween(ReviewRating.MinValue, ReviewRating.MaxValue);
        RuleFor(x => x.Comment)
            .MaximumLength(ReviewComment.MaxLength)
            .When(x => x.Comment is not null);
    }
}
