using System.ComponentModel.DataAnnotations;

namespace API_de_Gestión_de_Usuarios.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string Rol { get; set; } = "user";
    }
}
