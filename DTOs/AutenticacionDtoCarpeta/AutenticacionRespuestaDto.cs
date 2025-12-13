using API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta;

namespace API_de_Gestión_de_Usuarios.DTOs.AutenticacionDtoCarpeta
{
    public class AutenticacionRespuestaDto
    {
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; }
        public int TiempoExpiracionMinutos { get; set; }
    }
}
