using CSharpEssentials;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;

namespace MuhammedTask.Services.ProductService.Domain.Categories.Repositories;
public interface ICategoryCommandRepository
{
    Task<Result> DeleteProductsByCategoryIdAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}
