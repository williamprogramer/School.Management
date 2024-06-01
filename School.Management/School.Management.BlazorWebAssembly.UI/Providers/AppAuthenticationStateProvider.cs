using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace School.Management.BlazorWebAssembly.UI.Providers
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            List<Claim> claims = [new Claim(ClaimTypes.Name, "William"), new Claim(ClaimTypes.Role, "Administrator")];
            ClaimsIdentity identity = new (claims, "Custom");
            ClaimsPrincipal principal = new(identity);
            return Task.FromResult(new AuthenticationState(principal));
        }
    }
}