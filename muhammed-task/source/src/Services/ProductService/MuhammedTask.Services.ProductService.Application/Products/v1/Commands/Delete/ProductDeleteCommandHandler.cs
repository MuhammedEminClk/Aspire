using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;
using MuhammedTask.Services.ProductService.Domain.Products.Repositories;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Commands.Delete;

internal sealed class ProductDeleteCommandHandler(
    IProductCommandRepository repository) : ICommandHandler<ProductDeleteCommand>
{
    public Task<Result> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        return repository.DeleteProductAsync(productId, cancellationToken);
    }
}
