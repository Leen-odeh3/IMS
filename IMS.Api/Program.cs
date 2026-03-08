using IMS.Api.DI;
using IMS.Api.Middleware;
using IMS.Application.DI;
using IMS.Infrastructure.DependancyInjection;
using IMS.Infrastructure.ServiceContainer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .addInfraDependancy(builder.Configuration)
    .AddApplicationDependancy(builder.Configuration)
    .AddSwaggerDocumentation()
    .AddCorsPolicy()
    .AddObservability(builder.Configuration);

//
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Seq("http://seq:80")
    .CreateLogger();

builder.Host.UseSerilog();
//
var app = builder.Build();
await app.InitializeDatabaseAsync();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
app.UseSerilogRequestLogging();
app.UseObservability();
app.MapMetrics();
app.MapHealthChecks();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
