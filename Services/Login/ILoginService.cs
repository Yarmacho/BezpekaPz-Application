using Microsoft.AspNetCore.Http;

namespace Application.Services.Login
{
    public interface ILoginService
    {
        bool IsLoggedIn();

        LoginResult Login(string login, string password);

        RegisterResult Register(string name, string login, string password);

        ExistsResult Exists(string login);
        RestoreResult Restore(string login, string password);

        string CurrentUser();

        bool Logout();
    }
}
