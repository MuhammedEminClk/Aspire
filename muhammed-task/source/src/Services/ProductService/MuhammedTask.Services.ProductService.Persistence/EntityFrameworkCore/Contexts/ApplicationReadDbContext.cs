using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL.Contexts;
using MuhammedTask.Services.ProductService.Domain.Products.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductService.Persistence.EntityFrameworkCore.Contexts;
public sealed class ApplicationReadDbContext : ReadDbContextBase<ApplicationReadDbContext>
{
    public ApplicationReadDbContext(
        DbContextOptions<ApplicationReadDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }

    public DbSet<ProductReadModel> Products => Set<ProductReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
