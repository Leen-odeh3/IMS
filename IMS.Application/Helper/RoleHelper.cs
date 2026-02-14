using IMS.Core.Constants;

namespace IMS.Core.Helper;
public static class RoleHelper
{
    private static readonly string[] ValidRoles =
    {
        StaticRole.Admin,
        StaticRole.Manager,
        StaticRole.TeamLead,
        StaticRole.Trainer,
        StaticRole.Trainee,
        StaticRole.Hr
    };

    public static bool IsValidRole(string role)
    {
        return ValidRoles
            .Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
    }

    public static string NormalizeRole(string role)
    {
        return ValidRoles
            .First(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
    }

    public static IReadOnlyList<string> GetAllRoles()
        => ValidRoles;
}