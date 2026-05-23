using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;
using MuhammedTask.Services.CategoryService.Domain.Categories.Parameters;
using MuhammedTask.Services.CategoryService.Domain.Categories.Repositories;
namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Commands.Create;

internal sealed class CategoryCreateCommandHandler(
    ICategoryCommandRepository repository) : ICommandHandler<CategoryCreateCommand, CategoryId>
{
    public Task<Result<CategoryId>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        CategoryCreateParameters parameters = request.ToParameters();
        return repository.CreateCategoryAsync(parameters, cancellationToken);
    }
}
