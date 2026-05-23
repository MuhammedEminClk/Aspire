using MediatR;
using MuhammedTask.BuildingBlocks.Caching.Base;
using MuhammedTask.Services.ProductService.Domain.Products.Events;
using Microsoft.Extensions.Logging;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Events;

internal sealed class ProductDeletedDomainEventHandler(
    ILogger<ProductDeletedDomainEventHandler> logger,
    ICacheService cacheService) : INotificationHandler<ProductDeletedDomainEvent>
{
    private const string Tag = "products:list";
    public Task Handle(ProductDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductDeletedDomainEvent handled");
        string productIdCache = $"product:{notification.Id}";
        cacheService.Remove(productIdCache);
        return cacheService.InvalidateTagAsync(Tag);
    }
}
