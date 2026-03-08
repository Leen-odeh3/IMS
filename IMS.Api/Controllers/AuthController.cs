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

    [Authorize(Roles = StaticRole.SuperAdmin + "," + StaticRole.Admin)]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.Success)
        {
            _logger.LogWarning("Register failed for {Email}", dto.Email);
            return BadRequest(result.Message);
        }
        _logger.LogInformation("User {Email} created successfully", dto.Email);
        return Created(string.Empty, result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for user {Email}", dto.Email);
            return Unauthorized(result.Message);
        }
        _logger.LogInformation("User {Email} logged in successfully", dto.Email);

        return Ok(result.Data);
    }
}
