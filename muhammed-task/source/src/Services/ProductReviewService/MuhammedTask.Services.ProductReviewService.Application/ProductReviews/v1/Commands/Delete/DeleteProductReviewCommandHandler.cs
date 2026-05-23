using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Delete;

internal sealed class DeleteProductReviewCommandHandler(
    IProductReviewCommandRepository repository) : ICommandHandler<DeleteProductReviewCommand>
{
    public Task<Result> Handle(DeleteProductReviewCommand request, CancellationToken cancellationToken) =>
        repository.DeleteProductReviewAsync(
            ProductReviewId.From(request.Id),
            UserId.From(request.UserId),
            cancellationToken);
}
