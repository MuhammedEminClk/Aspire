using CSharpEssentials.Time;
using MuhammedTask.BuildingBlocks.MessageBrokers.Base;
using MuhammedTask.IntegrationEvents.Jobs;
using Microsoft.Extensions.Logging;
using Quartz;

namespace MuhammedTask.Services.CategoryService.Persistence.Jobs.PeriodicMessagePublisher;

public sealed class PeriodicMessagePublisherJob(
    IEventBus eventBus,
    ILogger<PeriodicMessagePublisherJob> logger,
    IDateTimeProvider dateTimeProvider
) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Periodic message publisher job is running {InstanceId}", context.FireInstanceId);
        await eventBus.PublishAsync(new PeriodicIntegrationEvent(context.FireInstanceId, dateTimeProvider.UtcNowDateTime), isTransactional: false);
    }
}
