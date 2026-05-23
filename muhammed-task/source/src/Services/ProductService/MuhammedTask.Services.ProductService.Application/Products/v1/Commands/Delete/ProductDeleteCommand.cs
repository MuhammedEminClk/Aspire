using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Commands.Delete;
public sealed record ProductDeleteCommand(Guid ProductId) : ICommand;
