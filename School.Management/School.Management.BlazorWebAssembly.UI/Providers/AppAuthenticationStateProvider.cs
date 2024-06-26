using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace School.Management.BlazorWebAssembly.UI.Providers
{
    public class AppAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal principal = new();
            if (!await localStorage.ContainKeyAsync("token"))
                return new AuthenticationState(principal);
            else
            {
                string? token = await localStorage.GetItemAsync<string>("token");
                HttpResponseMessage response = await httpClient.GetAsync($"api/authentication/validate/{token}");
                if (response.IsSuccessStatusCode)
                {
                    JsonWebTokenHandler tokenHandler = new();
                    JsonWebToken jwt = tokenHandler.ReadJsonWebToken(token);
                    ClaimsIdentity identity = new(jwt.Claims, "Jwt");
                    principal.AddIdentity(identity);
                    return new AuthenticationState(principal);
                }
                else
                    return new AuthenticationState(new ClaimsPrincipal());
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

        public async Task<IEnumerable<Claim>> GetClaims()
        {
            string? token = await localStorage.GetItemAsync<string>("token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                JsonWebTokenHandler tokenHandler = new();
                JsonWebToken jwt = tokenHandler.ReadJsonWebToken(token);
                return jwt.Claims;
            }
            else
                return Enumerable.Empty<Claim>();
        }
    }
}