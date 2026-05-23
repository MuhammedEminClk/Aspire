using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Configurations.Write;

internal sealed class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.SoftDeletableEntityBaseMap<ProductReview, ProductReviewId>();

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, id => ProductReviewId.From(id));

        builder
            .Property(p => p.ProductId)
            .HasConversion(id => id.Value, id => ProductId.From(id));

        builder
            .Property(p => p.UserId)
            .HasConversion(id => id.Value, id => UserId.From(id));

        builder
            .Property(p => p.Rating)
            .HasConversion(r => r.Value, r => ReviewRating.From(r));

        builder
            .Property(p => p.Comment)
            .HasConversion(c => c.Value, c => ReviewComment.From(c))
            .HasMaxLength(ReviewComment.MaxLength)
            .IsRequired(false);

        builder.OptimisticConcurrencyVersionMap();

        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => new { x.ProductId, x.UserId }).IsUnique();
        builder.HasIndex(x => new { x.CreatedAt, x.IsDeleted });
    }
}
