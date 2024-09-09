using GameCritic.Security;
using Microsoft.AspNetCore.Mvc;

namespace GameCritic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public JwtTokensController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetToken")]
        public ActionResult GetToken()
        {
            try
            {
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
