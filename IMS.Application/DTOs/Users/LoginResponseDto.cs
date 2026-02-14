
namespace IMS.Application.DTOs.Users;
public class LoginResponseDto
{
    public string Token { get; set; }
    public UserResponse User { get; set; }
}