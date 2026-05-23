using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Application.Products.v1.Models;
using MuhammedTask.Services.ProductService.Domain.Products;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;
using MuhammedTask.Services.ProductService.Domain.Products.ReadModels;
using MuhammedTask.Services.ProductService.Domain.Products.Repositories;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Queries.Get;

internal sealed class GetProductQueryHandler(
    IProductQueryRepository productQueryRepository) : ICachedQueryHandler<GetProductQuery, ProductViewModel>
{
    public async Task<Result<ProductViewModel>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.From(request.ProductId);
        Maybe<ProductReadModel> product = await productQueryRepository.GetProductByIdAsync(productId, cancellationToken);

        return product.Match<Result<ProductViewModel>>(
            value => ProductViewModel.Create(value),
            () => ProductErrors.ProductDoesNotExistError(productId));
    }
}
