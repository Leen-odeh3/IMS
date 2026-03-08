using IMS.Application.Common;
using IMS.Application.DTOs.Users;
namespace IMS.Application.Abstractions;
public interface IAuthService
{
    Task<Result<UserResponse>> RegisterAsync(RegisterUserDto dto);
    Task<Result<LoginResponseDto>> LoginAsync(LoginUserDto dto);
}
