using API_de_Gestión_de_Usuarios.Models;
using Microsoft.EntityFrameworkCore;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerUsuarioPorEmail(string email)
        {
            var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            return usuarioEncontrado;
        }
    }
}
