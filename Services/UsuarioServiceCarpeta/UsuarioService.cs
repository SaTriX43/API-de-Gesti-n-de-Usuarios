using API_de_Gestión_de_Usuarios.DALs;
using API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta;
using API_de_Gestión_de_Usuarios.Models;
using API_de_Gestión_de_Usuarios.Models.Enums;
using Microsoft.Identity.Client;

namespace API_de_Gestión_de_Usuarios.Services.UsuarioServiceCarpeta
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<List<UsuarioDto>>> ObtenerUsuariosAsync(int usuarioId,string rol)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorIdAsync(usuarioId);

            if(usuarioExiste == null)
            {
                return Result<List<UsuarioDto>>.Failure($"Su usuario con id = {usuarioId} no existe");
            }

            if (rol != RolUsuario.Admin.ToString())
            {
                return Result<List<UsuarioDto>>.Failure($"Su usuario con id = {usuarioId} no es Admin");
            }

            var usuarios = await _usuarioRepository.ObtenerUsuariosAsync();

            var usuariosDtos = usuarios.Select(u => new UsuarioDto
            {
                Email = u.Email,
                Id = u.Id,
                Nombre = u.Nombre,
                Rol = u.Rol.ToString()
            }).ToList();


            return Result<List<UsuarioDto>>.Success(usuariosDtos);
        }
        //public Task<Result<UsuarioDto>> ObtenerUsuarioAsync(int usuarioId);
    }
}
