using IMS.Api.DI;
using IMS.Api.Middleware;
using IMS.Application.DI;
using IMS.Infrastructure.DependancyInjection;
using IMS.Infrastructure.ServiceContainer;

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


var app = builder.Build();
await app.InitializeDatabaseAsync();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
app.UseObservability();
app.MapMetrics();
app.MapHealthChecks();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
