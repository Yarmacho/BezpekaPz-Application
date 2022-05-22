using Application.Models;
using Application.Services.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        
        private readonly ILoginService _loginService;
        public HomeController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [Route("")]
        public IActionResult Index()
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Login");
            }
            ViewData["Login"] = _loginService.CurrentUser();
            return View();
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            if (!_loginService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
