using Application.DbContexts;
using Application.Services.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Linq;
using System.Text;

namespace Application.Controllers
{
    [Route("files")]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationContext _db;
        public FilesController(ApplicationContext appContext)
        {
            _db = appContext;
        }

        [HttpPost("download")]
        public IActionResult DownloadFile([FromForm]IFormFile uploadedFile)
        {
            var login = (HttpContext.RequestServices.GetService(typeof(ILoginService)) as ILoginService)?.CurrentUser();
            if (!string.IsNullOrEmpty(login))
            {
                using (var reader = new StreamReader(uploadedFile.OpenReadStream(), System.Text.Encoding.UTF8))
                {
                    var data = System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd());

                    var fileModel = new Models.File() 
                    {
                        Content = data,
                        UserLogin = login,
                        Name = uploadedFile.FileName
                    };

                    _db.Files.Add(fileModel);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return BadRequest();
        }

        [HttpGet("upload/{id}")]
        public IActionResult Upload(int id)
        {
            var file = _db.Files.FirstOrDefault(f => f.Id == id);
            if (file != null && new FileExtensionContentTypeProvider().TryGetContentType(file.Name, out string contentType))
            {
                return File(file.Content, contentType, file.Name);
            }

            return BadRequest();
        }

        [HttpPost("save/{name?}")]
        public bool Save(string content, string name)
        {
            var login = (HttpContext.RequestServices.GetService(typeof(ILoginService)) as ILoginService)?.CurrentUser();
            if (string.IsNullOrEmpty(login))
            {
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "anon.txt";
            }

            var file = new Models.File()
            {
                Name = name,
                UserLogin = login,
                Content = Encoding.UTF8.GetBytes(content)
            };
            _db.Files.Add(file);
            return _db.SaveChanges() > 0;
        }
    }
}
