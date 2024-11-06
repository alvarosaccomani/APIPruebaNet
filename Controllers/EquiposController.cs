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
    }
}
