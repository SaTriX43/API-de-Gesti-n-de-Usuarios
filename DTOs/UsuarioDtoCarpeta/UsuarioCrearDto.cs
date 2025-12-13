using System.ComponentModel.DataAnnotations;

namespace API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta
{
    public class UsuarioCrearDto
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 
    }
}
