using System.ComponentModel.DataAnnotations;

namespace School.Management.BlazorWebAssembly.UI.Pages.Modules.Authentication.Models
{
    public class AutheticateModel
    {
        [Required(ErrorMessage = "usuario requerido")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "contraseña requerido")]
        public string Password { get; set; } = string.Empty;
    }
}