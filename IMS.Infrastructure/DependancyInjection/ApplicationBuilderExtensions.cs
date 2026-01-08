
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;

namespace IMS.Infrastructure.DependancyInjection;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseObservability(
        this IApplicationBuilder app)
    {
        app.UseHttpMetrics();
        return app;
    }
}
