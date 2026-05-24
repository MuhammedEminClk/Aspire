using FluentValidation;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Delete;

internal sealed class DeleteProductReviewCommandValidator : AbstractValidator<DeleteProductReviewCommand>
{
    public DeleteProductReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
