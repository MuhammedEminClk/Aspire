using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Update;

internal sealed class UpdateProductReviewCommandHandler(
    IProductReviewCommandRepository repository) : ICommandHandler<UpdateProductReviewCommand>
{
    public Task<Result> Handle(UpdateProductReviewCommand request, CancellationToken cancellationToken) =>
        repository.UpdateProductReviewAsync(request.ToParameters(), cancellationToken);
}
