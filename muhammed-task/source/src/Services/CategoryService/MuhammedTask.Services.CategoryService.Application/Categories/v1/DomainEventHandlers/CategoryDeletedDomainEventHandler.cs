using MediatR;
using MuhammedTask.BuildingBlocks.Caching.Base;
using MuhammedTask.BuildingBlocks.MessageBrokers.Base;
using MuhammedTask.IntegrationEvents.Categories;
using MuhammedTask.Services.CategoryService.Domain.Categories.Events;
using Microsoft.Extensions.Logging;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.DomainEventHandlers;

internal sealed class CategoryDeletedDomainEventHandler(
    ILogger<CategoryDeletedDomainEventHandler> logger,
    IEventBus eventBus,
    ICacheService cacheService) : INotificationHandler<CategoryDeletedDomainEvent>
{
    public async Task Handle(CategoryDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("CategoryDeletedDomainEvent handled");
        string CategoryIdCache = $"category:{notification.Id}";
        cacheService.Remove(CategoryIdCache);
        await eventBus.PublishAsync(new CategoryDeletedIntegrationEvent(notification.Id.Value), isTransactional: false, cancellationToken);
    }
}
