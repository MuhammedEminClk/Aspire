using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace MuhammedTask.BuildingBlocks.MessageBrokers.MassTransit.EntityFrameworkCore.Extensions;
public static class EfCoreDbContextExtensions
{
    public static ModelBuilder AddMassTransitOutboxEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();

        return modelBuilder;
    }
}
