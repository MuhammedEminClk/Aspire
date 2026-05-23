using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL.Contexts;
using MuhammedTask.Services.CategoryService.Domain.Categories.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
public sealed class ApplicationReadDbContext : ReadDbContextBase<ApplicationReadDbContext>
{
    public ApplicationReadDbContext(
        DbContextOptions<ApplicationReadDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }
    public DbSet<CategoryReadModel> Categories => Set<CategoryReadModel>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
