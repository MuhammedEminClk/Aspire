using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;
using MuhammedTask.Services.CategoryService.Domain.Categories.Parameters;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Commands.Create;

public sealed record CategoryCreateCommand(
    string? Name) : ICommand<CategoryId>
{
    public CategoryCreateParameters ToParameters() =>
        new(Name);
}
