using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL.Contexts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Contexts;

public sealed class ApplicationWriteDbContext : WriteDbContextBase<ApplicationWriteDbContext>
{
    public ApplicationWriteDbContext(
        DbContextOptions<ApplicationWriteDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }

    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
