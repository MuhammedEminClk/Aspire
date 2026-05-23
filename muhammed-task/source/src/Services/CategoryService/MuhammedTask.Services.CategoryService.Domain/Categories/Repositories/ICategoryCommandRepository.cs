using CSharpEssentials;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;
using MuhammedTask.Services.CategoryService.Domain.Categories.Parameters;

namespace MuhammedTask.Services.CategoryService.Domain.Categories.Repositories;
public interface ICategoryCommandRepository
{
    Task<Result<CategoryId>> CreateCategoryAsync(CategoryCreateParameters parameters, CancellationToken cancellationToken = default);
    Task<Result> DeleteCategoryAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}
