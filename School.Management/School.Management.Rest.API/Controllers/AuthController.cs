using Microsoft.AspNetCore.Mvc;

namespace School.Management.Rest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("success");
        }
    }
}