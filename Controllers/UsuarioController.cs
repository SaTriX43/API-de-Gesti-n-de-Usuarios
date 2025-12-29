using API_de_Gestión_de_Usuarios.Services.UsuarioServiceCarpeta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_de_Gestión_de_Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpGet("obtener-usuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId)) {
                return BadRequest(new
                {
                    success = false,
                    error = "usuarioId debe de ser un numero"
                });
            }

            var usuarios = await _usuarioService.ObtenerUsuariosAsync(usuarioId, rol);

            if(usuarios.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = usuarios.Error
                });
            }

            return Ok(new
            {
                success = true,
                value = usuarios.Value
            });
        }
    }
}
