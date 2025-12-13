using API_de_Gestión_de_Usuarios.DTOs.AutenticacionDtoCarpeta;
using API_de_Gestión_de_Usuarios.DTOs.UsuarioDtoCarpeta;
using API_de_Gestión_de_Usuarios.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_de_Gestión_de_Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioCrearDto usuarioCrear)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ModelState
                });
            }

            var usuarioCreado = await _autenticacionService.Registrar(usuarioCrear);

            if(usuarioCreado.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = usuarioCreado.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = usuarioCreado.Value
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ModelState
                });
            }

            var usuarioLogeado = await _autenticacionService.Login(loginDto);

            if(usuarioLogeado.IsFailure)
            {
                return NotFound(new
                {
                    success = false,
                    error = usuarioLogeado.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = usuarioLogeado.Value
            });
        }
    }
}
