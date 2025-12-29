using API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta;
using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.Services.UsuarioServiceCarpeta
{
    public interface IUsuarioService
    {
        public Task<Result<List<UsuarioDto>>> ObtenerUsuariosAsync(int usuarioId,string rol);
        //public Task<Result<UsuarioDto>> ObtenerUsuarioAsync(int usuarioId);
    }
}
