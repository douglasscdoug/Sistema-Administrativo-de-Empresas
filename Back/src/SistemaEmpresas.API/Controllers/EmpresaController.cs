using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Interfaces;

namespace SistemaEmpresas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var empresas = await _empresaService.GetAllAsync();
            return Ok(empresas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var empresa = await _empresaService.GetByIdAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return Ok(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmpresaRequestDto empresaRequestDto)
        {
            if(empresaRequestDto == null)
                return BadRequest();

            var empresa = await _empresaService.AddAsync(empresaRequestDto);
            
            return CreatedAtAction(nameof(GetById), new { id = empresa.Id }, empresa);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, EmpresaRequestDto empresaRequestDto)
        {
            if(empresaRequestDto == null)
                return BadRequest();

            var empresa = await _empresaService.UpdateAsync(id, empresaRequestDto);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmpresa(Guid id)
        {
            var deleted = await _empresaService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
