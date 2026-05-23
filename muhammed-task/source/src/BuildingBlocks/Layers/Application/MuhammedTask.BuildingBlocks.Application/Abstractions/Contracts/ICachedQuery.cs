namespace MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICacheable;
