using IMS.Application.Abstractions;
using IMS.Application.Common;
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
    public async Task<Result<bool>> AssignRoleAsync(AppUser user, string role)
    {
        if (!RoleHelper.IsValidRole(role))
            return Result<bool>.Fail("Invalid role.");

        var normalizedRole = RoleHelper.NormalizeRole(role);

        var result = await _userManager.AddToRoleAsync(user, normalizedRole);

        if (!result.Succeeded)
            return Result<bool>.Fail(
                string.Join(",", result.Errors.Select(e => e.Description)));

        return Result<bool>.Ok(true);
    }
}