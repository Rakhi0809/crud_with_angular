using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using web_API_Crud_operation_with_Angular.Helpers;

namespace web_API_Crud_operation_with_Angular.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        private readonly EncryptionHelper _encryptionHelper;

        public EncryptionController(IConfiguration configuration)
        {
            var encryptionKey = configuration["ConnectionStrings:EncryptionKey"];
            _encryptionHelper = new EncryptionHelper(encryptionKey);
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return BadRequest("Input text cannot be empty.");

            var encryptedText = _encryptionHelper.Encrypt(plainText);
            return Ok(new { EncryptedText = encryptedText });
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return BadRequest("Input text cannot be empty.");

            try
            {
                var decryptedText = _encryptionHelper.Decrypt(cipherText);
                return Ok(new { DecryptedText = decryptedText });
            }
            catch
            {
                return BadRequest("Invalid encrypted text.");
            }
        }
    }
}
