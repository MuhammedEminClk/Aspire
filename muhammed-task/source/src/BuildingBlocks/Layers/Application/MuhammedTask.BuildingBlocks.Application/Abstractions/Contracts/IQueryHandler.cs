using CSharpEssentials;
using MediatR;

namespace MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result> where TQuery : IQuery;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
