using API_de_Gestión_de_Usuarios.DALs;
using API_de_Gestión_de_Usuarios.DTOs.AutenticacionDtoCarpeta;
using API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta;
using API_de_Gestión_de_Usuarios.Models;
using API_de_Gestión_de_Usuarios.Services.IJwtServiceCarpeta;
using Microsoft.AspNetCore.Components.Forms;

namespace API_de_Gestión_de_Usuarios.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IAutenticacionRepository _autenticacionRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AutenticacionService(
            IAutenticacionRepository autenticacionRepository, 
            IConfiguration configuration, 
            IUsuarioRepository usuarioRepository,
            IJwtService jwtService
        )
        {
            _autenticacionRepository = autenticacionRepository;
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }



        public async Task<Result<AutenticacionRespuestaDto>> Registrar(UsuarioCrearDto usuarioCrear)
        {
            var emailNormalizado = usuarioCrear.Email.Trim().ToLower();
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorEmail(emailNormalizado);

            if(usuarioExiste != null)
            {
                return Result<AutenticacionRespuestaDto>.Failure("No se pudo crear usuario con la informacion dada");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(usuarioCrear.Password);

            var usuarioModel = new Usuario
            {
                Nombre = usuarioCrear.Nombre,
                Email = emailNormalizado,
                PasswordHash = passwordHash,
                Rol = "user"
            };

            var usuarioCreadoModel = await _autenticacionRepository.Registrar(usuarioModel);

            var usuarioCreadoDto = new UsuarioDto
            {
                Nombre = usuarioCreadoModel.Nombre,
                Email = usuarioCreadoModel.Email,
                Id = usuarioCreadoModel.Id,
                Rol = usuarioCreadoModel.Rol
            };

            var token = _jwtService.GenerarToken(usuarioCreadoModel);

            var tokenMinutos = _configuration.GetValue<int>("Jwt:AccessTokenMinutes");

            return Result<AutenticacionRespuestaDto>.Success(new AutenticacionRespuestaDto
            {
                Token = token,
                Usuario = usuarioCreadoDto,
                TiempoExpiracionMinutos = tokenMinutos
            });
        }

        public async Task<Result<AutenticacionRespuestaDto>> Login(LoginDto login)
        {
            var emailNormalizado = login.Email.Trim().ToLower();
            var usuarioEncontrado = await _usuarioRepository.ObtenerUsuarioPorEmail(emailNormalizado);

            if(usuarioEncontrado == null)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Credenciales invalidas");
            }

            var esValido = BCrypt.Net.BCrypt.Verify(login.Password, usuarioEncontrado.PasswordHash);

            if(!esValido)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Credenciales Invalidas");
            }

            var token = _jwtService.GenerarToken(usuarioEncontrado);

            var usuarioDto = new UsuarioDto
            {
                Id = usuarioEncontrado.Id,
                Email = usuarioEncontrado.Email,
                Nombre = usuarioEncontrado.Nombre,
                Rol = usuarioEncontrado.Rol,
            };

            return Result<AutenticacionRespuestaDto>.Success(new AutenticacionRespuestaDto
            {
                Token = token,
                Usuario = usuarioDto,
                TiempoExpiracionMinutos = _configuration.GetValue<int>("Jwt:AccessTokenMinutes")
            });
        }
    }
}
