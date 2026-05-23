using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.ProductService.Application.Products.v1.Models;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Queries.Get;
public sealed record GetProductQuery(Guid ProductId) : ICachedQuery<ProductViewModel>
{
    public bool BypassCache => false;

    public bool CacheFailures => true;

    public string CacheKey => $"product:{ProductId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => [];
}
