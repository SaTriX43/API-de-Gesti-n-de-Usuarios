using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.Services.IJwtServiceCarpeta
{
    public interface IJwtService
    {
        public string GenerarToken(Usuario usuario);
    }
}
