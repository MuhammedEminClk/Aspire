using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

public sealed record ProductReviewUpdateParameters(
    ProductReviewId Id,
    UserId UserId,
    int? Rating,
    string? Comment);
