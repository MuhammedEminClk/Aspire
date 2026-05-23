using CSharpEssentials.EntityFrameworkCore;
using MuhammedTask.BuildingBlocks.Database.PostgreSQL.Contexts;
using MuhammedTask.Services.CategoryService.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MuhammedTask.BuildingBlocks.MessageBrokers.MassTransit.EntityFrameworkCore.Extensions;

namespace MuhammedTask.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
public sealed class ApplicationWriteDbContext : WriteDbContextBase<ApplicationWriteDbContext>
{
    public ApplicationWriteDbContext(
        DbContextOptions<ApplicationWriteDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
        modelBuilder.AddMassTransitOutboxEntities();
    }
}
