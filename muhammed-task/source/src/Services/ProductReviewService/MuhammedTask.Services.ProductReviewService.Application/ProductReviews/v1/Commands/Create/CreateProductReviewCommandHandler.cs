using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Create;

internal sealed class CreateProductReviewCommandHandler(
    IProductReviewCommandRepository repository,
    IProductReviewQueryRepository queryRepository) : ICommandHandler<CreateProductReviewCommand, ProductReviewId>
{
    public async Task<Result<ProductReviewId>> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.Product);
        var userId = UserId.From(request.User);

        bool alreadyReviewed = await queryRepository.ExistsAsync(productId, userId, cancellationToken);
        if (alreadyReviewed)
            return Domain.ProductReviews.ProductReviewErrors.AlreadyReviewedError;

        return await repository.CreateProductReviewAsync(request.ToParameters(), cancellationToken);
    }
}
