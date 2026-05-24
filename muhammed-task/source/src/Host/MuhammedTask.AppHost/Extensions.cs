using HealthChecks.NpgSql;
using HealthChecks.Redis;
using HealthChecks.Uris;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MuhammedTask.AppHost;
public static class Extensions
{
    public static IResourceBuilder<RabbitMQServerResource> WithHealthCheck(this IResourceBuilder<RabbitMQServerResource> builder)
    {
        // WithManagementPlugin() registers HealthCheckAnnotation(key: "{name}_check") and also
        // calls AddHealthChecks().AddRabbitMQ() which uses RabbitMQ.Client 6.x API.
        // Since Directory.Packages.props pins RabbitMQ.Client to 7.x (required by MassTransit 8.x),
        // the built-in check crashes at runtime. We replace it with an HTTP-based check against
        // the management API, which is version-agnostic.
        string key = $"{builder.Resource.Name}_check";

        // Remove the broken check registered by WithManagementPlugin and replace it.
        builder.ApplicationBuilder.Services
            .Configure<HealthCheckServiceOptions>(opts =>
            {
                HealthCheckRegistration? existing = opts.Registrations
                    .FirstOrDefault(r => r.Name == key);
                if (existing is not null)
                    opts.Registrations.Remove(existing);

                opts.Registrations.Add(new HealthCheckRegistration(key, sp => new RabbitMQManagementHealthCheck(builder.Resource), null, null));
            });

        return builder;
    }

    public static IResourceBuilder<RedisResource> WithHealthCheck(this IResourceBuilder<RedisResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new RedisHealthCheck(cs)));
    }

    public static IResourceBuilder<PostgresServerResource> WithHealthCheck(this IResourceBuilder<PostgresServerResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new NpgSqlHealthCheck(new NpgSqlHealthCheckOptions(cs))));
    }

    public static IResourceBuilder<T> WithHealthCheck<T>(
        this IResourceBuilder<T> builder,
        string? endpointName = null,
        string path = "health",
        Action<UriHealthCheckOptions>? configure = null)
        where T : IResourceWithEndpoints
    {
        return builder.WithAnnotation(new HealthCheckAnnotation((resource, ct) =>
        {
            if (resource is not IResourceWithEndpoints resourceWithEndpoints)
            {
                return Task.FromResult<IHealthCheck?>(null);
            }

            EndpointReference? endpoint = endpointName is null
             ? resourceWithEndpoints.GetEndpoints().FirstOrDefault(e => e.Scheme is "http" or "https")
             : resourceWithEndpoints.GetEndpoint(endpointName);

            string? url = endpoint?.Url;

            if (url is null)
            {
                return Task.FromResult<IHealthCheck?>(null);
            }

            var options = new UriHealthCheckOptions();

            options.AddUri(new(new(url), path));

            configure?.Invoke(options);

            var client = new HttpClient();
            return Task.FromResult<IHealthCheck?>(new UriHealthCheck(options, () => client));
        }));
    }
}

internal sealed class RabbitMQManagementHealthCheck(RabbitMQServerResource resource) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        EndpointReference? endpoint = resource.GetEndpoints()
            .FirstOrDefault(e => e.Scheme is "http" or "https");

        if (endpoint?.Url is not string baseUrl)
            return HealthCheckResult.Unhealthy("RabbitMQ management endpoint not available.");

        string? password = resource.PasswordParameter is IValueProvider p
            ? await p.GetValueAsync(cancellationToken)
            : null;

        string credentials = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes($"guest:{password}"));

        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
        try
        {
            HttpResponseMessage response = await client.GetAsync(
                new Uri(new Uri(baseUrl), "api/health/checks/alarms"), cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy($"HTTP {(int)response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
}

public class HealthCheckAnnotation(Func<IResource, CancellationToken, Task<IHealthCheck?>> healthCheckFactory) : IResourceAnnotation
{
    public Func<IResource, CancellationToken, Task<IHealthCheck?>> HealthCheckFactory { get; } = healthCheckFactory;

    public static HealthCheckAnnotation Create(Func<string, IHealthCheck> connectionStringFactory)
    {
        return new(async (resource, token) =>
        {
            if (resource is not IResourceWithConnectionString c)
            {
                return null;
            }

            if (await c.GetConnectionStringAsync(token) is not string cs)
            {
                return null;
            }

            return connectionStringFactory(cs);
        });
    }
}
