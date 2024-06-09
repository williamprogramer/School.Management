using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Blazored.LocalStorage;

namespace School.Management.BlazorWebAssembly.UI.Providers
{
    public class AppAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            bool hasToken = await localStorage.ContainKeyAsync("token");
            if (!hasToken)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }
            else
            {
                string? token = await localStorage.GetItemAsync<string>("token");
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
                ClaimsIdentity identity = new(tokenValidation.ClaimsIdentity.Claims, "JWT");
                ClaimsPrincipal principal = new(identity);
                return await Task.FromResult(new AuthenticationState(principal));
            }
        }

        public async Task AuthenticateAsync(string token)
        {
            await localStorage.SetItemAsync("token", token);
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
            ClaimsIdentity identity = new(tokenValidation.ClaimsIdentity.Claims, "JWT");
            ClaimsPrincipal principal = new(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("token");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}