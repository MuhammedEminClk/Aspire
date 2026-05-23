using CSharpEssentials;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;
using MuhammedTask.Services.ProductService.Domain.Products.ReadModels;

namespace MuhammedTask.Services.ProductService.Domain.Products.Repositories;
public interface IProductQueryRepository
{
    Task<Maybe<ProductReadModel>> GetProductByIdAsync(ProductId productId, CancellationToken cancellationToken = default);
    Task<ProductReadModel[]> GetProductsByCategoryId(CategoryId categoryId, CancellationToken cancellationToken = default);
}
