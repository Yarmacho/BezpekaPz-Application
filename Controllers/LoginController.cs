using Application.Services;
using Application.Services.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Controllers
{
    [Route("")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View(TempData.TryGetValue("result", out object result) 
                ? JsonConvert.DeserializeObject<LoginResult>(result?.ToString()) : null);
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View(TempData.TryGetValue("result", out object result) 
                ? JsonConvert.DeserializeObject<RegisterResult>(result?.ToString()) : null);
        }

        [Route("forgot")]
        public IActionResult Forgot()
        {
            return View(TempData.TryGetValue("result", out object result) 
                ? JsonConvert.DeserializeObject<RequestResult>(result?.ToString()) : null);
        }

        [HttpPost("login")]
        public IActionResult Login(string login, string password)
        {
            var result = _loginService.Login(login, password);
            if (!result.Success)
            {
                return new JsonResult(result);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("register")]
        public IActionResult Register(string name, string login, string password)
        {
            var result = _loginService.Register(name, login, password);
            if (!result.Success)
            {
                TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Register");
            }
            return RedirectToAction("Login");
        }

        [HttpPost("exists")]
        public IActionResult Exists(string login)
        {
            var result = _loginService.Exists(login);
            TempData["result"] = JsonConvert.SerializeObject(result);
            if (!result.Success)
            {
                return RedirectToAction("Forgot");
            }
            return RedirectToAction("Forgot");
        }

        [HttpPost("restore")]
        public IActionResult Restore([FromForm]string login, [FromForm] string password)
        {
            var result = _loginService.Restore(login, password);
            if (!result.Success)
            {
                TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Forgot");
            }
            return RedirectToAction("Login");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            if (_loginService.Logout())
            {
                return RedirectToAction("Login", "Login");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
