using CSharpEssentials;
using MediatR;

namespace MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
