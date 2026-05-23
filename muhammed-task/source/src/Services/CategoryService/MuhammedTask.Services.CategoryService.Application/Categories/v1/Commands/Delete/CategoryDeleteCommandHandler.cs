using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;
using MuhammedTask.Services.CategoryService.Domain.Categories.Repositories;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Commands.Delete;

internal sealed class CategoryDeleteCommandHandler(
    ICategoryCommandRepository repository) : ICommandHandler<CategoryDeleteCommand>
{
    public Task<Result> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var productId = CategoryId.From(request.CategoryId);
        return repository.DeleteCategoryAsync(productId, cancellationToken);
    }
}
