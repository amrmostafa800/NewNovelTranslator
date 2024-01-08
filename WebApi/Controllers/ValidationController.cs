using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        [HttpGet("IsAuth")]
        public IActionResult IsAuth()
        {
            if (string.IsNullOrEmpty(Request.Headers.Cookie)) 
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
