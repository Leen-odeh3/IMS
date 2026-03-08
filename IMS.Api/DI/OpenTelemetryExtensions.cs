using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

namespace IMS.Api.DI;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddObservability(
      this IServiceCollection services,
      IConfiguration configuration)
    {
        var otlpEndpoint = new Uri(configuration["Otlp:Endpoint"]);

        var resource = ResourceBuilder.CreateDefault()
            .AddService("IMS.API");

        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(resource)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = otlpEndpoint;
                    });
            });

        //services.AddLogging(logging =>
        //{
        //    logging.AddOpenTelemetry(options =>
        //    {
        //        options.SetResourceBuilder(resource);

        //        options.IncludeFormattedMessage = true;
        //        options.IncludeScopes = true;
        //        options.ParseStateValues = true;

        //        options.AddOtlpExporter(o =>
        //        {
        //            o.Endpoint = otlpEndpoint;
        //        });
        //    });
        //});

        return services;
    }

}
