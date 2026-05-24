namespace MuhammedTask.IntegrationEvents.ProductReviews;

public sealed record ProductReviewCreatedIntegrationEvent(
    Guid ProductId,
    double NewAverageRating,
    int ReviewCount);
