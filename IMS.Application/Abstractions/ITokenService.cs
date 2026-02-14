using IMS.Core.Entities;
namespace IMS.Application.Abstractions;
public interface ITokenService
{
    Task<string> GenerateTokenAsync(AppUser user);
}
