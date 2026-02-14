using IMS.Application.Abstractions;
using IMS.Core.Constants;
using IMS.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]   
[Authorize(Roles = StaticRole.Admin)]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly UserManager<AppUser> _userManager;

    public RolesController(IRoleService roleService, UserManager<AppUser> userManager)
    {
        _roleService = roleService;
        _userManager = userManager;
    }

    [Authorize(Roles = StaticRole.SuperAdmin)]
    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromQuery] int userId, [FromQuery] string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("User not found.");

        await _roleService.AssignRoleAsync(user, role);
        return Ok(new { Message = $"Role '{role}' assigned to user {userId}" });
    }
}
