
using IMS.Core.Entities;

namespace IMS.Application.Abstractions;
public interface IRoleService
{
    Task AssignRoleAsync(AppUser user, string role);
}