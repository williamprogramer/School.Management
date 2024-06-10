using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace School.Management.Rest.API.Controllers
{
    public class AuthRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Response
    {
        public string FullName { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private string GenerateToken(string name, string role)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("gEWsrPC7h5oxD9LkniwHOMd4TeN2acB6"));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new(ClaimTypes.Role, role),
                new(ClaimTypes.Name, name)
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
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest request)
        {
            try
            {
                using SqlConnection sqlConnection = new("Data Source=;Initial Catalog=;Integrated Security=true;TrustServerCertificate=True;");
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                DynamicParameters parameters = new();
                parameters.Add("@password", request.Password, DbType.String);
                string query = "select [fullname] from [Security].[User] where convert(varchar, DECRYPTBYPASSPHRASE(@password, [Password])) = @password;";
                Response? response = sqlConnection.QueryFirstOrDefault<Response>(query, parameters);
                sqlConnection.Close();

                if (response is not null)
                {
                    string token = GenerateToken(response.FullName, "Administrator");
                    return Ok(new { Token = token });
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}