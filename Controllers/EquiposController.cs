using Microsoft.AspNetCore.Mvc;
using APIPruebaNet.Models;
using APIPruebaNet.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIPruebaNet.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly EquipoService _equipoService;
        public EquiposController(EquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
        {
            var equipos = await _equipoService.GetEquiposAsync();
            return Ok(equipos);
        }

        [HttpGet("{eq_Id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int eq_Id)
        {
            var equipo = await _equipoService.GetEquipoAsync(eq_Id);
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        public async Task<ActionResult> PostEquipo(Equipo equipo)
        {
            await _equipoService.AddEquipoAsync(equipo);
            return CreatedAtAction(nameof(GetEquipo), new { eq_id = equipo.eq_Id }, equipo);
        }

        [HttpPut("{eq_Id}")]
        public async Task<ActionResult> PutEquipo(int eq_Id, Equipo equipo)
        {
            if (eq_Id != equipo.eq_Id)
            {
                return BadRequest();
            }
            await _equipoService.ActualizarEquipoAsync(eq_Id, equipo);
            return NoContent();
        }

        [HttpDelete("{eq_Id}")]
        public async Task<IActionResult> DeleteEquipo(int eq_Id)
        {
            await _equipoService.EliminaEquipoAsync(eq_Id);
            return NoContent();
        }

    }
}
