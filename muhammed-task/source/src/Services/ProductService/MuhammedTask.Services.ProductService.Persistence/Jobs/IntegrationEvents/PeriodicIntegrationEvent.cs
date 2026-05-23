namespace MuhammedTask.IntegrationEvents.Jobs;

public sealed record PeriodicIntegrationEvent(string JobInstanceId, DateTime Timestamp);