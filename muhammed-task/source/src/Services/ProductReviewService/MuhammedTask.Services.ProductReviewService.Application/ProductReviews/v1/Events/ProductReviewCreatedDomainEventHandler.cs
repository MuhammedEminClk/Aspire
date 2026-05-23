using MediatR;
using MuhammedTask.BuildingBlocks.Caching.Base;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Events;
using Microsoft.Extensions.Logging;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Events;

internal sealed class ProductReviewCreatedDomainEventHandler(
    ILogger<ProductReviewCreatedDomainEventHandler> logger,
    ICacheService cacheService) : INotificationHandler<ProductReviewCreatedDomainEvent>
{
    private const string Tag = "reviews";

    public async Task Handle(ProductReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductReviewCreatedDomainEvent handled for review {ReviewId}", notification.Id.Value);
        await cacheService.InvalidateTagAsync(Tag);
    }
}
