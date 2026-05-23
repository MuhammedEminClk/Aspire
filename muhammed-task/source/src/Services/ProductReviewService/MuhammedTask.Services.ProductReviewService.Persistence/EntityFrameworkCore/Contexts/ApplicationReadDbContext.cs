using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL.Contexts;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Contexts;

public sealed class ApplicationReadDbContext : ReadDbContextBase<ApplicationReadDbContext>
{
    public ApplicationReadDbContext(
        DbContextOptions<ApplicationReadDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }

    public DbSet<ProductReviewReadModel> ProductReviews => Set<ProductReviewReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
