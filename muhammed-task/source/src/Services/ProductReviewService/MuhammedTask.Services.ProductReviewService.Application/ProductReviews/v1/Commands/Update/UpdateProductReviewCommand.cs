using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Update;

public sealed record UpdateProductReviewCommand(
    Guid Id,
    Guid User,
    int? Rating,
    string? Comment) : ICommand
{
    public ProductReviewUpdateParameters ToParameters() =>
        new(ProductReviewId.From(Id), UserId.From(User), Rating, Comment);
}
