using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public interface IAutenticacionRepository
    {
        public Task<Usuario> Registrar(Usuario usuario);
    }
}
