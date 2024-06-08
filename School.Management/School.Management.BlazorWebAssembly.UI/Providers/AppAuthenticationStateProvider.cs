using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace School.Management.BlazorWebAssembly.UI.Providers
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
        }

        public async Task AuthenticateAsync(string token)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("gEWsrPC7h5oxD9LkniwHOMd4TeN2acB6"));
            TokenValidationParameters validationParameters = new()
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = "https://localhost:7265",
                ValidateAudience = true,
                ValidAudience = "http://localhost:5263"
            };
            JsonWebTokenHandler tokenHandler = new();
            TokenValidationResult tokenValidation = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            List<Claim> claims = [new Claim(ClaimTypes.Name, "William"), new Claim(ClaimTypes.Role, "Administrator")];
            ClaimsIdentity identity = new(claims, "Custom");
            ClaimsPrincipal principal = new(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public void Logout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}