using System.ComponentModel.DataAnnotations;

namespace School.Management.BlazorWebAssembly.UI.Pages.Modules.Maintenance.Administrator.Models
{
    public class AddAdministratorFormModel : IDisposable
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido")]
        public string Mail { get; set; } = string.Empty;
        [Required(ErrorMessage = "La fecha de nacimiento es requerido")]
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "El numero de identificacion es requerido")]
        public string IdNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "La direccion es requerido")]
        public string Address { get; set; } = string.Empty;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.WaitForPendingFinalizers();
        }
    }
}