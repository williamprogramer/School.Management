using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using School.Management.Rest.API.Features.Authentication.Commands;
using System.Security.Claims;
using System.Text;

namespace School.Management.Rest.API.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(this AuthenticateResponse response, IConfiguration configuration)
        {
            try
            {
                string? key = configuration.GetValue<string>("Jwt:Key");
                string? issuer = configuration.GetValue<string>("Jwt:Issuer");

                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer))
                    throw new Exception("Propiedades necesarias para el token descriptor incorrectas");

                SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
                SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

                Claim[] claims =
                [
                    new(JwtRegisteredClaimNames.Sub, response.Id.ToString()),
                    new(JwtRegisteredClaimNames.Name, response.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Role, response.RolName),
                    new(ClaimTypes.Name, response.FullName)
                ];

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Issuer = issuer,
                    Audience = "http://localhost:5263",
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = credentials
                };

                JsonWebTokenHandler tokenHandler = new();
                return tokenHandler.CreateToken(tokenDescriptor);
            }
            catch
            {
                throw;
            }
        }
    }
}