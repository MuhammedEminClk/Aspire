using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Domain.Categories.Rules.Exist;
using MuhammedTask.Services.ProductService.Domain.Categories.Services;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;
using MuhammedTask.Services.ProductService.Domain.Products.Parameters;
using MuhammedTask.Services.ProductService.Domain.Products.Repositories;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Commands.Create;

internal sealed class ProductCreateCommandHandler(
    IProductCommandRepository repository,
    ICategoryService categoryService) : ICommandHandler<ProductCreateCommand, ProductId>
{
    public Task<Result<ProductId>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var rule = new CategoryExistRule(categoryService);
        ProductCreateParameters parameters = request.ToParameters();
        return repository.CreateProductAsync(parameters, rule, cancellationToken);
    }
}
