using CSharpEssentials.Interfaces;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;

public sealed class ProductReviewReadModel : ISoftDeletableBase
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
