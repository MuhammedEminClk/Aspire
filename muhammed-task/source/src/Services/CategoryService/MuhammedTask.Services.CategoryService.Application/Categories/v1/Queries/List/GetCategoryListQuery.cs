using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;
using MuhammedTask.Services.CategoryService.Application.Products.v1.Models;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Queries.List;
public sealed record GetCategoryListQuery() :
    ICachedQuery<CategoryViewModel[]>
{
    public bool BypassCache => false;

    public bool CacheFailures => true;

    public string CacheKey => $"categories";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => [];
}
