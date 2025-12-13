using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public class AutenticacionRepository : IAutenticacionRepository
    {
        private readonly ApplicationDbContext _context;

        public AutenticacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Registrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
