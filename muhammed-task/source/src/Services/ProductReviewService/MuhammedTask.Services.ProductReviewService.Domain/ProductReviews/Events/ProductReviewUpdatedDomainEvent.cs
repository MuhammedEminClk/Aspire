using CSharpEssentials.Interfaces;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Events;

public sealed record ProductReviewUpdatedDomainEvent(ProductReviewId Id) : IDomainEvent;
