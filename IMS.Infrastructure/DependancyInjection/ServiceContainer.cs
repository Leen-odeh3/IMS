
using IMS.Application.Abstractions;
using IMS.Core.Entities;
using IMS.Infrastructure.Database;
using IMS.Infrastructure.DbInitilizer;
using IMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace IMS.Infrastructure.ServiceContainer;
public static class ServiceContainer
{
    public static IServiceCollection addInfraDependancy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddIdentityServices(services);
        AddHealthChecks(services, configuration);
        AddRepositories(services);

        return services;
    }
    private static void AddDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IAppDbContext>(sp =>
            sp.GetRequiredService<AppDbContext>());
    }
    private static void AddIdentityServices(IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 0;
        })
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<AppDbContext>();
    }
    private static void AddHealthChecks(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                tags: new[] { "db", "ready" }
            );
    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IDBInitilizer, DBInitilizer>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
