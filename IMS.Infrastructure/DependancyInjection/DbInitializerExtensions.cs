using IMS.Infrastructure.DbInitilizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure.DependancyInjection;

public static class DbInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IDBInitilizer>();
        await initializer.Initialize();
    }
}