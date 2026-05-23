using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;
using MuhammedTask.Services.ProductService.Domain.Products.Parameters;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Commands.Create;

public sealed record ProductCreateCommand(
    string? Name,
    string? Description,
    decimal Price,
    Currency Currency,
    Guid Category) : ICommand<ProductId>
{
    public ProductCreateParameters ToParameters() =>
        new(Name, Description, Price, Currency, CategoryId.From(Category));
}
