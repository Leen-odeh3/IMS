using IMS.Application.DTOs.Users;
using IMS.Core.Entities;

namespace IMS.Application.Handlers;
public interface IRoleProfileHandler
{
    string Role { get; }
    Task HandleAsync(RegisterUserDto dto, AppUser user);
}