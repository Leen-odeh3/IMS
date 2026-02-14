using IMS.Application.DTOs.Users;
namespace IMS.Application.Abstractions;
public interface IAuthService
{
    Task<UserResponse> RegisterAsync(RegisterUserDto dto);
    Task<LoginResponseDto> LoginAsync(LoginUserDto dto);
}
