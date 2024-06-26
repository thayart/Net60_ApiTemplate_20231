using Net60_ApiTemplate_20231.DTOs.Auth;

namespace Net60_ApiTemplate_20231.Services.Auth
{
    public interface ILoginDetailServices
    {
        string Token { get; }

        string[] Roles { get; }

        string[] Permissions { get; }

        LoginDetailDto GetClaim();

        bool CheckPermission(string permission);

        bool CheckRole(string role);
    }
}