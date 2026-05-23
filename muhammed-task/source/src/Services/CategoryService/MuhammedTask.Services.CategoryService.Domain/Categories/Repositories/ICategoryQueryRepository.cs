using CSharpEssentials;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;
using MuhammedTask.Services.CategoryService.Domain.Categories.ReadModels;

namespace MuhammedTask.Services.CategoryService.Domain.Categories.Repositories;
public interface ICategoryQueryRepository
{
    Task<Maybe<CategoryReadModel>> GetCategoryByIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
    Task<CategoryReadModel[]> GetCategories(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}
