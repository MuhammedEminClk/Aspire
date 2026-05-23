using System.Reflection;
using Carter;
using CSharpEssentials;
using CSharpEssentials.AspNetCore;
using CSharpEssentials.RequestResponseLogging;
using MuhammedTask.BuildingBlocks.Application.Shared.Constants;
using MuhammedTask.BuildingBlocks.Presentation.Authentication;
using MuhammedTask.BuildingBlocks.Presentation.Cors;
using MuhammedTask.BuildingBlocks.Presentation.HealthChecks;
using MuhammedTask.BuildingBlocks.Presentation.SessionContexts;
using MuhammedTask.Services.Info;
using MuhammedTask.Services.ProductService.Application.ServiceRegistrations;
using MuhammedTask.Services.ProductService.Persistence.ServiceRegistrations;
using Microsoft.Net.Http.Headers;

namespace MuhammedTask.Services.ProductService.WebApi.ServiceRegistrations;

internal static class DependencyInjection
{
    internal static IServiceCollection AddServiceRegistrations(
        this IServiceCollection services,
        IHostEnvironment hostEnvironment,
        IConfiguration configuration)
    {
        //services.AddControllers();
        //services.AddRouting();
        services.AddCarter();
        services.AddHealthChecks();

        return services
            .AddAllAcceptCors()
            .AddHttpContextAccessor()
            .AddApplicationServices()
            .AddPersistenceServices(hostEnvironment, configuration)
            .AddSessionContext()
            .ConfigureHttpClients()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .ConfigureModelValidatorResponse()
            .ConfigureSystemTextJson()
            .AddEnhancedProblemDetails()
            .AddAndConfigureApiVersioning()
            .AddSwagger<DefaultConfigureSwaggerOptions>(SecuritySchemes.JwtBearerTokenSecurity, Assembly.GetExecutingAssembly())
            .AddKeycloakJwtBearer(keycloakServiceId: ServiceKeys.Keycloak, realm: "products")
            .ConfigureTelemetries(hostEnvironment);
    }

    internal static WebApplication UseServices(
        this WebApplication app)
    {
        app.UseVersionableSwagger();
        app.AddRequestResponseLogging(opt =>
        {
            opt.IgnorePaths("/health");
            var loggingOptions = LoggingOptions.CreateAllFields();
            loggingOptions.HeaderKeys.Add(HeaderNames.AcceptLanguage);
            loggingOptions.HeaderKeys.Add(CustomHeaderNames.TenantId);
            opt.UseLogger(app.Services.GetRequiredService<ILoggerFactory>(), loggingOptions);
        });
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseCors("all");
        //app.UseRouting();
        //app.MapControllers();
        app.MapCarter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseDefaultHealthChecks();

        return app;
    }
}
