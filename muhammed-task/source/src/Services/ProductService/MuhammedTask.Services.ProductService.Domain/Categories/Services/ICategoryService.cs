using CSharpEssentials;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;

namespace MuhammedTask.Services.ProductService.Domain.Categories.Services;
public interface ICategoryService
{
    Task<Result<bool>> CategoryExistsAsync(CategoryId categoryId, CancellationToken cancellationToken = default);
}
