
using IMS.Application.Common;
using IMS.Core.Entities;

namespace IMS.Application.Abstractions;
public interface IRoleService
{
    Task<Result<bool>> AssignRoleAsync(AppUser user, string role);
}