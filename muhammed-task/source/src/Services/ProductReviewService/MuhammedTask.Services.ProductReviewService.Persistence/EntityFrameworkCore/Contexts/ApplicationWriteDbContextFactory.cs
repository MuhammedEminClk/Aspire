using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Contexts;

internal sealed class ApplicationWriteDbContextFactory : IDesignTimeDbContextFactory<ApplicationWriteDbContext>
{
    public ApplicationWriteDbContext CreateDbContext(string[] args)
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__pg-productreviewservice")
            ?? "Host=localhost;Database=productreview;Username=postgres";

        ServiceCollection services = new();
        services.AddLogging();
        services.AddDbContext<ApplicationWriteDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention());

        IServiceProvider provider = services.BuildServiceProvider();

        return provider.GetRequiredService<ApplicationWriteDbContext>();
    }
}
