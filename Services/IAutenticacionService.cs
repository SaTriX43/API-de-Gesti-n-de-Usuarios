using API_de_Gestión_de_Usuarios.DTOs.AutenticacionDtoCarpeta;
using API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta;
using API_de_Gestión_de_Usuarios.Models;

namespace API_de_Gestión_de_Usuarios.Services
{
    public interface IAutenticacionService
    {
        public Task<Result<AutenticacionRespuestaDto>> Registrar(UsuarioCrearDto usuarioCrear);
        public Task<Result<AutenticacionRespuestaDto>> Login(LoginDto login);
        public Task<Result<AutenticacionRespuestaDto>> RefreshToken(RefreshTokenDto token);
        public Task<Result> Logout(RefreshTokenDto token);
    }
}
