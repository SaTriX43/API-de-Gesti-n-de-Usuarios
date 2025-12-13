using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public interface IUsuarioRepository
    {
        public Task<Usuario?> ObtenerUsuarioPorEmail(string email);
    }
}
