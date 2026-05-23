using CSharpEssentials;

namespace MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;

public readonly record struct UserId
{
    private UserId(Guid value) => Value = value;
    public Guid Value { get; }
    public static UserId New() => new(Guider.NewGuid());
    public static UserId From(Guid value) => new(value);
    public static readonly UserId Empty = new(Guid.Empty);
}
