using API_de_Gestión_de_Usuarios.Models.Enums;
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
        public RolUsuario Rol { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();    
    }
}
