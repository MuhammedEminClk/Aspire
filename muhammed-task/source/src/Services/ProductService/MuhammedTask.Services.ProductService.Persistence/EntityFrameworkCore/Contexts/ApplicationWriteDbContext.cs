using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL.Contexts;
using MuhammedTask.Services.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductService.Persistence.EntityFrameworkCore.Contexts;
public sealed class ApplicationWriteDbContext : WriteDbContextBase<ApplicationWriteDbContext>
{
    public ApplicationWriteDbContext(
        DbContextOptions<ApplicationWriteDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
