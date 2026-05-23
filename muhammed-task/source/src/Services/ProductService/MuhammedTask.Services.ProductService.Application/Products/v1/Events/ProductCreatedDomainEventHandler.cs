using MediatR;
using MuhammedTask.BuildingBlocks.Caching.Base;
using MuhammedTask.Services.ProductService.Domain.Products.Events;
using Microsoft.Extensions.Logging;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Events;
internal sealed class ProductCreatedDomainEventHandler(
    ILogger<ProductCreatedDomainEventHandler> logger,
    ICacheService cacheService) : INotificationHandler<ProductCreatedDomainEvent>
{
    private const string Tag = "products";

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductCreatedDomainEvent handled");
        await cacheService.InvalidateTagAsync(Tag);
    }
}
