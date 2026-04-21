using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.API.Extensions;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Filters;
using SistemaEmpresas.Application.Interfaces;

namespace SistemaEmpresas.API.Controllers
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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UsuarioFiltroDto filtro)
        {
            var result = await _usuarioService.Filtrar(filtro);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (User.GetUserRole() != "Administrador" &&
                User.GetUserId() != id.ToString()) return Forbid();

            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioRequestDto dto)
        {
            if (dto == null) return BadRequest();

            var usuario = await _usuarioService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = usuario.Id },
                usuario
            );
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UsuarioRequestDto dto)
        {
            var usuarioExistente = await _usuarioService.GetByIdAsync(id);
            if (usuarioExistente == null) return NotFound();

            if (User.GetUserRole() != "Administrador" &&
                User.GetUserId() != id.ToString()) return Forbid();

            if(User.GetUserRole() != "Administrador")
            {
                dto.Role = usuarioExistente.Role;
            }
                
            if (dto == null) return BadRequest();

            var usuario = await _usuarioService.UpdateAsync(id, dto);
            if (usuario == null) return NotFound();

            return Ok(usuario);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _usuarioService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
