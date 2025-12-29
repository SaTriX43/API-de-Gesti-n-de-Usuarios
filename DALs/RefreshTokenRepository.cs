using API_de_Gestión_de_Usuarios.Models;
using Microsoft.EntityFrameworkCore;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> CrearRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
        public async Task<RefreshToken?> ObtenerRefreshToken(string refreshToken)
        {
            var refreshTokenEncontrado = await _context.RefreshTokens.Include(rt => rt.Usuario).FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            return refreshTokenEncontrado;
        }

        public async Task RevocarRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.RevocadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
