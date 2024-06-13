using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace School.Management.BlazorWebAssembly.UI.Providers
{
    public class AppAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!await localStorage.ContainKeyAsync("token"))
                return new AuthenticationState(new ClaimsPrincipal());
            else
            {
                string? token = await localStorage.GetItemAsync<string>("token");
                JsonWebTokenHandler tokenHandler = new();
                JsonWebToken jwt = tokenHandler.ReadJsonWebToken(token);
                ClaimsIdentity identity = new(jwt.Claims, "Jwt");
                ClaimsPrincipal principal = new(identity);
                return new AuthenticationState(principal);
            }
        }

        public async Task AuthenticateAsync(string token)
        {
            JsonWebTokenHandler handler = new();
            JsonWebToken jwt = handler.ReadJsonWebToken(token);
            ClaimsPrincipal principal = new(new ClaimsIdentity(jwt.Claims, "Jwt"));
            await localStorage.SetItemAsync("token", token);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task LogoutAsync()
        {
            await localStorage.RemoveItemAsync("token");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}