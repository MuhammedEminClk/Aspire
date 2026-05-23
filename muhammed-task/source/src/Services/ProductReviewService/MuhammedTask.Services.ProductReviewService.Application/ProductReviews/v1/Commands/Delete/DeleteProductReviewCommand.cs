using MuhammedTask.BuildingBlocks.Application.Abstractions.Contracts;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Delete;

public sealed record DeleteProductReviewCommand(Guid Id, Guid UserId) : ICommand;
