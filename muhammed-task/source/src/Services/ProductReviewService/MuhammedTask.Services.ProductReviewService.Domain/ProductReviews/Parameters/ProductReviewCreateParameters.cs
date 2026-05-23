using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;

public sealed record ProductReviewCreateParameters(
    ProductId ProductId,
    UserId UserId,
    int? Rating,
    string? Comment);
