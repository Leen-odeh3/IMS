

namespace IMS.Infrastructure.DependancyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;

public static class EndpointExtensions
{
    public static void MapObservabilityEndpoints(
        this WebApplication app)
    {
        app.MapMetrics("/metrics");

        app.MapHealthChecks("/health/live");
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });
    }
}
