using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Queries.Exist;
public sealed record CategoryExistQuery(Guid CategoryId) : IQuery<bool>;
