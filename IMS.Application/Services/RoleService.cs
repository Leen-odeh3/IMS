using IMS.Application.Abstractions;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using IMS.Core.Helper;
using Microsoft.AspNetCore.Identity;

namespace IMS.Application.Services;
public class RoleService : IRoleService
{
    private readonly UserManager<AppUser> _userManager;

    public RoleService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task AssignRoleAsync(AppUser user, string role)
    {
        if (!RoleHelper.IsValidRole(role))
            throw new BadRequestException("Invalid role.");

        var normalizedRole = RoleHelper.NormalizeRole(role);

        var result = await _userManager.AddToRoleAsync(user, normalizedRole);

        if (!result.Succeeded)
            throw new BadRequestException(
                string.Join(",", result.Errors.Select(e => e.Description)));
    }

}