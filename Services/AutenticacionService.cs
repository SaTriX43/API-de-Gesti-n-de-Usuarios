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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AutenticacionService(
            IRefreshTokenRepository refreshTokenRepository, 
            IConfiguration configuration, 
            IUsuarioRepository usuarioRepository,
            IJwtService jwtService
        )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }



        public async Task<Result<AutenticacionRespuestaDto>> Registrar(UsuarioCrearDto usuarioCrear)
        {
            var emailNormalizado = usuarioCrear.Email.Trim().ToLower();
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorEmailAsync(emailNormalizado);

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

            var usuarioCreadoModel = await _usuarioRepository.Registrar(usuarioModel);

            var nuevoRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = usuarioCreadoModel.Id,
                CreadoEn = DateTime.UtcNow,
                ExpiraEn = DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("Jwt:RefreshTokenDays")
                 ),
                Usado = false
            };

            var refreshTokenCreado = await _refreshTokenRepository.CrearRefreshToken(nuevoRefreshToken);


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
                RefreshToken = refreshTokenCreado.Token,
                Usuario = usuarioCreadoDto,
                TiempoExpiracionMinutos = tokenMinutos
            });
        }

        public async Task<Result<AutenticacionRespuestaDto>> Login(LoginDto login)
        {
            var emailNormalizado = login.Email.Trim().ToLower();
            var usuarioEncontrado = await _usuarioRepository.ObtenerUsuarioPorEmailAsync(emailNormalizado);

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

            var nuevoRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = usuarioEncontrado.Id,
                CreadoEn = DateTime.UtcNow,
                ExpiraEn = DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("Jwt:RefreshTokenDays")
                 ),
                Usado = false
            };

            var refreshTokenCreado = await _refreshTokenRepository.CrearRefreshToken(nuevoRefreshToken);

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
                RefreshToken = refreshTokenCreado.Token,
                Usuario = usuarioDto,
                TiempoExpiracionMinutos = _configuration.GetValue<int>("Jwt:AccessTokenMinutes")
            });
        }

        public async Task<Result<AutenticacionRespuestaDto>> RefreshToken(RefreshTokenDto token)
        {
            var tokenEncontrado = await _refreshTokenRepository.ObtenerRefreshToken(token.RefreshToken);

            if(tokenEncontrado == null)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Su refresh token no existe");
            }

            if(tokenEncontrado.RevocadoEn != null)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Su refresh token ya fue revocado");
            }

            if (tokenEncontrado.ExpiraEn < DateTime.UtcNow)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Su refresh token ya expiro");
            }

            if(tokenEncontrado.Usado)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Su refresh token ya ha sido usado");
            }

            if(tokenEncontrado.Usuario == null) 
            {
                return Result<AutenticacionRespuestaDto>.Failure($"El usuario asociado al refresh token no existe");
            }

            tokenEncontrado.Usado = true;
            tokenEncontrado.RevocadoEn = DateTime.UtcNow;

            var nuevoRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = tokenEncontrado.UserId,
                CreadoEn = DateTime.UtcNow,
                ExpiraEn = DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("Jwt:RefreshTokenDays")
                 ),
                Usado = false
            };

            var refreshTokenCreado = await _refreshTokenRepository.CrearRefreshToken(nuevoRefreshToken);

            var accessToken = _jwtService.GenerarToken(tokenEncontrado.Usuario);
            var usuarioDto = new UsuarioDto
            {
                Id = tokenEncontrado.UserId,
                Email = tokenEncontrado.Usuario.Email,
                Nombre = tokenEncontrado.Usuario.Nombre,
                Rol = tokenEncontrado.Usuario.Rol
            };

            return Result<AutenticacionRespuestaDto>.Success(new AutenticacionRespuestaDto
            {
                Token = accessToken,
                Usuario = usuarioDto,
                RefreshToken = refreshTokenCreado.Token,
                TiempoExpiracionMinutos = _configuration.GetValue<int>("Jwt:AccessTokenMinutes"),

            });
        }
        public async Task<Result> Logout(RefreshTokenDto token)
        {
            var tokenEncontrado = await _refreshTokenRepository.ObtenerRefreshToken(token.RefreshToken);

            if (tokenEncontrado == null)
            {
                return Result.Failure("Refresh token no existe");
            }

            if(tokenEncontrado.RevocadoEn != null) 
            {
                return Result.Failure("Su Refresh token ya ha sido revocado");
            }

            if(tokenEncontrado.ExpiraEn < DateTime.UtcNow)
            {
                return Result.Failure("Su Refresh token ya ha expirado ");
            }

            if(tokenEncontrado.Usuario == null)
            {
                return Result.Failure("Su Usuario no existe");
            }

            await _refreshTokenRepository.RevocarRefreshToken(tokenEncontrado);

            return Result.Success();
        }
    }
}
