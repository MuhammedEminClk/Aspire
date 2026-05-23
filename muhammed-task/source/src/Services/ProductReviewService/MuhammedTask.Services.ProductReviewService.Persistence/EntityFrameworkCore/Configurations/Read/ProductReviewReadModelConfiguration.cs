using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Configurations.Read;

public sealed class ProductReviewReadModelConfiguration : IEntityTypeConfiguration<ProductReviewReadModel>
{
    public void Configure(EntityTypeBuilder<ProductReviewReadModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
