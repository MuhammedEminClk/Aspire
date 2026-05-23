using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Application.Products.v1.Models;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;
using MuhammedTask.Services.ProductService.Domain.Products.ReadModels;
using MuhammedTask.Services.ProductService.Domain.Products.Repositories;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Queries.List;

internal sealed class GetProductListQueryHandler(
    IProductQueryRepository repository) : ICachedQueryHandler<GetProductListQuery, ProductViewModel[]>
{
    public async Task<Result<ProductViewModel[]>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.From(request.CategoryId);
        ProductReadModel[] products = await repository.GetProductsByCategoryId(categoryId, cancellationToken);
        ProductViewModel[] models = ProductViewModel.Create(products);
        return models;

    }
}
