
using Microsoft.AspNetCore.Identity;
using IMS.Core.Entities;
using IMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IMS.Infrastructure.DbInitilizer;

namespace IMS.Infrastructure.ServiceContainer;
public static class ServiceContainer
{
    public static IServiceCollection addInfraDependancy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IDBInitilizer, DBInitilizer>();


        return services;
    }
}
