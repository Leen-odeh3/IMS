using IMS.Application.DTOs.Users;
using IMS.Core.Entities;

namespace IMS.Application.Abstractions;
public interface IAuthService
{
    Task<AppUser> AddNewUserAsync(
        AppUser user,
        string password,
        string role,
        Trainer? trainer,
        Trainee? trainee);
}
