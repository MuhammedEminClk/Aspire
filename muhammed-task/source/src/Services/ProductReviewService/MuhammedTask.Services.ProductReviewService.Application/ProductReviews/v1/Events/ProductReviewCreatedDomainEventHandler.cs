using MediatR;
using MuhammedTask.BuildingBlocks.Caching.Base;
using MuhammedTask.BuildingBlocks.MessageBrokers.Base;
using MuhammedTask.IntegrationEvents.ProductReviews;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Events;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;
using Microsoft.Extensions.Logging;

namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Events;

internal sealed class ProductReviewCreatedDomainEventHandler(
    ILogger<ProductReviewCreatedDomainEventHandler> logger,
    IEventBus eventBus,
    ICacheService cacheService,
    IProductReviewQueryRepository queryRepository) : INotificationHandler<ProductReviewCreatedDomainEvent>
{
    private const string Tag = "reviews";

    public async Task Handle(ProductReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductReviewCreatedDomainEvent handled for review {ReviewId}", notification.Id.Value);

        await cacheService.InvalidateTagAsync(Tag);

        double averageRating = await queryRepository.GetAverageRatingByProductIdAsync(notification.ProductId, cancellationToken);
        int reviewCount = await queryRepository.GetReviewCountByProductIdAsync(notification.ProductId, cancellationToken);

        await eventBus.PublishAsync(
            new ProductReviewCreatedIntegrationEvent(notification.ProductId.Value, averageRating, reviewCount),
            isTransactional: false,
            cancellationToken);
    }
}
