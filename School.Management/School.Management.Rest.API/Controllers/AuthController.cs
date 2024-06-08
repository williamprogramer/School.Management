using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace School.Management.Rest.API.Controllers
{
    public class AuthRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest request)
        {
            if (request.Name == "amayawa" && request.Password == "gEWsrPC7h5oxD9LkniwHOMd4TeN2acB6")
            {
                SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("gEWsrPC7h5oxD9LkniwHOMd4TeN2acB6"));
                SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims =
                [
                    new(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Name, "William Amaya"),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Role, "Administrator"),
                    new(ClaimTypes.Name, "William Amaya")
                ];

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Issuer = "https://localhost:7265",
                    Audience = "http://localhost:5263",
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = credentials
                };

                JsonWebTokenHandler tokenHandler = new();
                string token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = token });
            }

            return NotFound();
        }
    }
}