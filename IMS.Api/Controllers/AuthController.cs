using IMS.Application.Abstractions;
using IMS.Application.DTOs.Users;
using IMS.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [Authorize(Roles = StaticRole.SuperAdmin)]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        _logger.LogInformation("Register request received for Email: {Email}", dto.Email);
        var response = await _authService.RegisterAsync(dto);
        _logger.LogInformation("User created successfully");
        return Created(string.Empty, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}
