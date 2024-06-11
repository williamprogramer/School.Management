using System.ComponentModel.DataAnnotations;

namespace School.Management.BlazorWebAssembly.UI.Pages.Modules.Authentication.Models
{
    public class AutheticateModel
    {
        [Required(ErrorMessage = "El Usuario es requerido")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "La Contraseña es requerido")]
        public string Password { get; set; } = string.Empty;
    }
}