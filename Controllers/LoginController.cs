using Application.Entities;
using Application.Services;
using Application.Services.Login;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.IO;

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
        public IActionResult Login([FromServices] RandomStringGenerator generator)
        {
            ViewData["captchaString"] = generator.Generate(8);
            return View(TempData.TryGetValue("result", out object result) 
                ? JsonConvert.DeserializeObject<LoginResult>(result?.ToString()) : null);
        }

        [Route("register")]
        public IActionResult Register([FromServices] RandomStringGenerator generator)
        {
            ViewData["captchaString"] = generator.Generate(8);
            return View(TempData.TryGetValue("result", out object result) 
                ? JsonConvert.DeserializeObject<RegisterResult>(result?.ToString()) : null);
        }

        [Route("forgot")]
        public IActionResult Forgot([FromServices] RandomStringGenerator generator)
        {
            ViewData["captchaString"] = generator.Generate(8);
            return View(TempData.TryGetValue("result", out object result) 
                ? JsonConvert.DeserializeObject<RequestResult>(result?.ToString()) : null);
        }

        [HttpPost("login")]
        public IActionResult Login(string login, string password, string captchaString, string userCaptcha)
        {
            var result = _loginService.Login(login, password);
            result.CaptchaSuccess = captchaString == userCaptcha;
            if (!result.Success || !result.CaptchaSuccess)
            {
                TempData["result"] = JsonConvert.SerializeObject(result);
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("captcha")]
        public IActionResult Captcha(string captchaString)
        {
            CaptchaImage captcha = new CaptchaImage(captchaString, 250, 50);

            using var stream = new MemoryStream();
            captcha.Image.Save(stream, ImageFormat.Jpeg);
           
            captcha.Dispose();
            return File(stream.ToArray(), "image/jpeg");
        }

        [HttpPost("register")]
        public IActionResult Register(string name, string login, string password,
            string captchaString, string userCaptcha)
        {
            var result = _loginService.Register(name, login, password);
            result.CaptchaSuccess = captchaString == userCaptcha;
            if (!result.Success || !result.CaptchaSuccess)
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
        public IActionResult Restore([FromForm]string login, [FromForm] string password,
            string captchaString, string userCaptcha)
        {
            var result = _loginService.Restore(login, password);
            result.CaptchaSuccess = captchaString == userCaptcha;
            if (!result.Success || !result.CaptchaSuccess)
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
