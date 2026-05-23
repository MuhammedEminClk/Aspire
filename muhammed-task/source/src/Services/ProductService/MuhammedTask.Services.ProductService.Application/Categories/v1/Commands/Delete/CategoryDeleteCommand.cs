using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

namespace MuhammedTask.Services.ProductService.Application.Categories.v1.Commands.Delete;
public sealed record CategoryDeleteCommand(Guid CategoryId) : ICommand;
