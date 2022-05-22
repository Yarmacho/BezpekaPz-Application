using Application.DbContexts;
using Application.Services.Login;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Application.Controllers
{
    [Route("")]
    public class UserController : Controller
    {
        private readonly ApplicationContext _db;
        public UserController(ApplicationContext context)
        {
            _db = context;
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var loginService = HttpContext.RequestServices.GetService(typeof(ILoginService)) as ILoginService;
            if (loginService == null)
            {
                return BadRequest();
            }
            var login = loginService.CurrentUser();
            ViewData["Login"] = login;

            return View(_db.Files.Where(file => file.UserLogin == login).ToList());
        }
    }
}
