
using IMS.Application.Abstractions;
using IMS.Core.Entities;
using IMS.Infrastructure.Database;
using IMS.Infrastructure.DbInitilizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IMS.Infrastructure.ServiceContainer;
public static class ServiceContainer
{
    public static IServiceCollection addInfraDependancy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IDBInitilizer, DBInitilizer>();

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 0;
        })
        .AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
        ;

        services.AddHealthChecks()
    .AddSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        tags: new[] { "db", "ready" }
    );

        return services;
    }
}
