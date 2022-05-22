using Application.DbContexts;
using Application.Services.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Application.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly string _userCookie;
        private readonly ApplicationContext _db;
        private readonly IEncryptionService _encryptionService;
        private readonly string _encryptionKey;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(ApplicationContext applicationContext, IEncryptionService encryptionService,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = applicationContext;
            _encryptionService = encryptionService;
            _encryptionKey = configuration.GetSection("Encryption")["Key"];
            _userCookie = configuration.GetSection("Auth")["CookieName"];
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(_userCookie);
            }
            return false;
        }

        public LoginResult Login(string login, string password)
        {
            var user = _db.Users.FirstOrDefault(user => user.Login == login);
            if (user == null)
            {
                return new LoginResult("No such user!");
            }
            else if (_encryptionService.Decrypt(user.Password, _encryptionKey) != password)
            {
                return new LoginResult("Incorrect password!");
            }

            if (_httpContextAccessor.HttpContext == null)
            {
                return new LoginResult("Something went wrong!");
            }
            login = _encryptionService.Encrypt(login, _encryptionKey);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(_userCookie, login);

            return new LoginResult() { Login = _encryptionService.Encrypt(login, _encryptionKey) };
        }

        public RegisterResult Register(string name, string login, string password)
        {
            var user = _db.Users.FirstOrDefault(user => user.Login == login);
            if (user != null)
            {
                return new RegisterResult("User is already registered");
            }

            user = new Models.User()
            {
                Login = login,
                Name = name,
                Password = _encryptionService.Encrypt(password, _encryptionKey)
            };

            _db.Users.Add(user);
            var affectedRows = _db.SaveChanges();

            return affectedRows > 0
                ? new RegisterResult()
                : new RegisterResult("Something went wrong!\nTry later");
        }

        public ExistsResult Exists(string login)
        {
            var user = _db.Users.FirstOrDefault(user => user.Login == login);
            if (user == null)
            {
                return new ExistsResult("No such user!");
            }


            var result = new ExistsResult();
            result.AddParameter("Login", login);
            return result;
        }

        public RestoreResult Restore(string login, string password)
        {
            var user = _db.Users.FirstOrDefault(user => user.Login == login);
            if (user == null)
            {
                var result = new RestoreResult("No such user!");
                result.AddParameter("Login", login);
                return result;
            }

            user.Password = _encryptionService.Encrypt(password, _encryptionKey);
            var affectedRows = _db.SaveChanges();
            return affectedRows > 0
                ? new RestoreResult()
                : new RestoreResult("Something went wrong!\nTry later");
        }

        public string CurrentUser()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                return context.Request.Cookies.TryGetValue(_userCookie, out string login)
                    ? _encryptionService.Decrypt(login, _encryptionKey)
                    : null;
            }

            return null;
        }

        public bool Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Cookies.Delete(_userCookie);
                return true;
            }
            return false;
        }
    }
}
