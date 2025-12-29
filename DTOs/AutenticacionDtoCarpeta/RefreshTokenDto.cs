using System.ComponentModel.DataAnnotations;

namespace API_de_Gestión_de_Usuarios.DTOs.AutenticacionDtoCarpeta
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
