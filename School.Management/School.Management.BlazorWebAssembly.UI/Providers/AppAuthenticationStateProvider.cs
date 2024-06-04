using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace School.Management.BlazorWebAssembly.UI.Providers
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
        }

        public void Authenticate()
        {
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