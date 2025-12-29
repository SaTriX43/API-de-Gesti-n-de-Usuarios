using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace API_de_Gestión_de_Usuarios.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime ExpiraEn {  get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime? RevocadoEn { get; set; }
        public bool Usado {  get; set; }
        [Required]
        public int UserId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
