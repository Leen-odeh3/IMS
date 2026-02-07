using IMS.Application.Abstractions;
using IMS.Application.DTOs.Users;
using IMS.Application.Mapper;
using IMS.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserMapper _mapper;

    public AuthController(IAuthService authService, UserMapper mapper)
    {
        _authService = authService; 
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var appUser = _mapper.MapToAppUser(dto);

        Trainer? trainer = null;
        Trainee? trainee = null;

        if (dto.Role == "Trainer")
        {
            trainer = _mapper.MapToTrainer(dto);
            trainer.User = appUser;
        }

        if (dto.Role == "Trainee")
        {
            trainee = _mapper.MapToTrainee(dto);
            trainee.User = appUser;
        }

        var user = await _authService.AddNewUserAsync(
            appUser,
            dto.Password,
            dto.Role,
            trainer,
            trainee);

        var response = _mapper.MapToResponse(user);

        response.Role = dto.Role;

        return Ok(response);
    }
}
