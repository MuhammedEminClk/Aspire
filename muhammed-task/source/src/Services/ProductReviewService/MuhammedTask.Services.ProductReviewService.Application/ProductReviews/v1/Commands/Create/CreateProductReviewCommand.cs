using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Create;

public sealed record CreateProductReviewCommand(
    Guid Product,
    Guid User,
    int? Rating,
    string? Comment) : ICommand<ProductReviewId>
{
    public ProductReviewCreateParameters ToParameters() =>
        new(ProductId.From(Product), UserId.From(User), Rating, Comment);
}
