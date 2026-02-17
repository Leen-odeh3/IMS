using IMS.Application.Abstractions;
using IMS.Application.DTOs.Users;
using IMS.Application.Handlers;
using IMS.Application.Mapper;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using IMS.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
namespace IMS.Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserMapper _mapper;
    private readonly IRoleService _roleService;
    private readonly ITokenService _tokenService;
    private readonly IEnumerable<IRoleProfileHandler> _roleHandlers;

    public AuthService(
        UserManager<AppUser> userManager,
        IUnitOfWork unitOfWork,
        ITraineeRepository traineeRepository,
        ITrainerRepository trainerRepository,
        UserMapper mapper,
        IRoleService roleService,
        ITokenService tokenService, IEnumerable<IRoleProfileHandler> roleHandlers)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _traineeRepository = traineeRepository;
        _trainerRepository = trainerRepository;
        _mapper = mapper;
        _roleService = roleService;
        _tokenService = tokenService;
        _roleHandlers = roleHandlers;
    }

    public async Task<UserResponse> RegisterAsync(RegisterUserDto dto)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                throw new BadRequestException("User already exists");

            var appUser = _mapper.MapToAppUser(dto);

            var result = await _userManager.CreateAsync(appUser, dto.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(e => e.Description));

            await _roleService.AssignRoleAsync(appUser, dto.Role);
            var handler = _roleHandlers
    .FirstOrDefault(h =>
        h.Role.Equals(dto.Role, StringComparison.OrdinalIgnoreCase));

            if (handler == null)
                throw new BadRequestException("Invalid role");

            await handler.HandleAsync(dto, appUser);

            var response = _mapper.MapToResponse(appUser);
            response.Role = dto.Role;
            return response;
        });
    }

     public async Task<LoginResponseDto> LoginAsync(LoginUserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, dto.Password) ||
            !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await _tokenService.GenerateTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var response = _mapper.MapToResponse(user);
        response.Role = roles.FirstOrDefault();

        return new LoginResponseDto
        {
            Token = token,
            User = response
        };
    }
}
