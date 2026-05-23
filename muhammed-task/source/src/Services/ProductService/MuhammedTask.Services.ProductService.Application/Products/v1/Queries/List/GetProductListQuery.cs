using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Application.Products.v1.Models;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Queries.List;
public sealed record GetProductListQuery(Guid CategoryId) :
    ICachedQuery<ProductViewModel[]>
{
    public bool BypassCache => false;

    public bool CacheFailures => true;

    public string CacheKey => $"category:{CategoryId}:products";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => [$"category:{CategoryId}", "products"];
}
