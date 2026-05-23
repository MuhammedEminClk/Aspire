using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Commands.Delete;
public sealed record CategoryDeleteCommand(Guid CategoryId) : ICommand;
