using Microsoft.AspNetCore.Mvc;
using APIPruebaNet.Models;
using APIPruebaNet.Services;
using System.Collections.Generic;
using System.Threading.Channels;
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
        public async Task<ActionResult> PostEquipo([FromBody] Equipo equipo)
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

        [HttpGet("stream")]
        public async Task Get([FromQuery] string prompt)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            var channel = Channel.CreateUnbounded<string>(); 
            _ = Task.Run(async () => 
            {
                await foreach (var message in GenerateResponseAsync(prompt)) 
                { 
                    await channel.Writer.WriteAsync($"data: {message}\n\n");
                }
                channel.Writer.Complete();
            });
            await foreach (var message in channel.Reader.ReadAllAsync())
            {
                await Response.WriteAsync(message);
                await Response.Body.FlushAsync();
            }
        }
        
        private async IAsyncEnumerable<string> GenerateResponseAsync(string prompt) {
            var messages = new List<string> {
                "Hola! Estoy aquí para ayudarte.", "¿Cómo puedo asistirte hoy?", "Espero que estés teniendo un buen día."
            };
            foreach (var message in messages)
            { 
                await Task.Delay(1000); // Simula un retraso de procesamiento
                yield return message;
            }
        }
    }
}
