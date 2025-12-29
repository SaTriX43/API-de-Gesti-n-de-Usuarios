using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public interface IUsuarioRepository
    {
        public Task<Usuario> Registrar(Usuario usuario);
        public Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email);
        public Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        public Task<List<Usuario>> ObtenerUsuariosAsync();
    }
}
