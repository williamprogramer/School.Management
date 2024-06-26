namespace School.Management.BlazorWebAssembly.UI.Pages.Modules.Authentication.Models
{
    public class AuthenticationResponseModel
    {
        public string Token { get; set; } = string.Empty;

        public bool IsValid() {
            return !string.IsNullOrWhiteSpace(Token);
        }
    }
}