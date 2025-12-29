using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.DALs
{
    public interface IRefreshTokenRepository
    {
        
        public Task<RefreshToken> CrearRefreshToken(RefreshToken refreshToken);
        public Task<RefreshToken?> ObtenerRefreshToken(string refreshToken);
        public Task RevocarRefreshToken(RefreshToken refreshToken);
    }
}
