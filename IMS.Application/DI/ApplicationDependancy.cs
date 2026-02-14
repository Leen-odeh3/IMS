using FluentValidation;
using IMS.Application.Abstractions;
using IMS.Application.Handlers;
using IMS.Application.Helper;
using IMS.Application.Mapper;
using IMS.Application.Services;
using IMS.Application.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application.DI;
public static class ApplicationDependancy
{
    public static IServiceCollection AddApplicationDependancy(this IServiceCollection services,IConfiguration config)
    {
        services.AddSingleton<UserMapper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();
        services.Configure<JwtOptions>(config.GetSection("Jwt"));
        services.AddValidatorsFromAssembly(typeof(AppUserValidator).Assembly);
        services.AddScoped<IRoleProfileHandler, TrainerProfileHandler>();
        services.AddScoped<IRoleProfileHandler, TraineeProfileHandler>();

        return services;
    }
}
