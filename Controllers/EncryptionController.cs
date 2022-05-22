using Application.Services.Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Controllers
{
    [ApiController]
    [Route("encryption")]
    public class EncryptionController : ControllerBase
    {
        private readonly IEncryptionService _encryptionService;
        private readonly string _encryptionKey;
        public EncryptionController(IEncryptionService encryptionService, IConfiguration configuration)
        {
            _encryptionService = encryptionService;
            _encryptionKey = configuration.GetSection("Encryption")["Key"];
        }

        [HttpPost("encrypt")]
        public string Encrypt([FromForm]string data)
        {
            return _encryptionService.Encrypt(data, _encryptionKey);
        }

        [HttpPost("decrypt")]
        public string Decrypt([FromForm] string data)
        {
            return _encryptionService.Decrypt(data, _encryptionKey);
        }
    }
}
