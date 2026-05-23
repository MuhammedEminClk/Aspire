using CSharpEssentials;
using MediatR;

namespace MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

public interface IQuery : IRequest<Result>;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
