using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TEMPERATURA_MAXIMA.Context;
using API_TEMPERATURA_MAXIMA.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace API_TEMPERATURA_MAXIMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperaturaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TemperaturaController(AppDbContext context)
        {
            _context = context;
        }
                   private int GetIdFuncionario()
        {
            // Recupera o IdFuncionario das claims do usuário autenticado
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        /// <summary>
        /// retorna as temperaturas registradas no banco de dados
        /// </summary>
        /// <summary>
        /// retorna as mudanças temperaturas registrados no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idTemperatura": 0,
        ///    "temperatura": 0,
        ///    "horarioTemp": "2024-10-10T18:36:58.699Z",
        ///    "id_ambiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/Temperatura
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Temperatura>>> GetTemperaturas()
        {
          if (_context.Temperaturas == null)
          {
              return NotFound();
          }
          var batata = GetIdFuncionario();
          Console.Write("", batata);
            return await _context.Temperaturas.ToListAsync();
        }

        /// <summary>
        /// retorna uma temperatura registrada no banco de dados em relação de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idTemperatura": 0,
        ///    "temperatura": 0,
        ///    "horarioTemp": "2024-10-10T18:36:58.699Z",
        ///    "id_ambiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/Temperatura/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Temperatura>>> GetTemperatura(int id)
        {
          if (_context.Temperaturas == null)
          {
              return NotFound();
          }
            var temperatura = await _context.Temperaturas.Where(e => e.IdAmbiente == id).ToListAsync();

            if (temperatura == null)
            {
                return NotFound();
            }
            return temperatura;
        }

        /// <summary>
        /// faz um update de dados da temperatura no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idTemperatura": 0,
        ///    "temperatura": 0,
        ///    "horarioTemp": "2024-10-10T18:36:58.699Z",
        ///    "id_ambiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no update dos dados</response>


        // PUT: api/Temperatura/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTemperatura(int id, Temperatura temperatura)
        {
            if (id != temperatura.IdTemperatura)
            {
                return BadRequest();
            }

            _context.Entry(temperatura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemperaturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// insere dados de temperatura no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idTemperatura": 0,
        ///    "temperatura": 0,
        ///    "horarioTemp": "2024-10-10T18:36:58.699Z",
        ///    "id_ambiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>

        // POST: api/Temperatura
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        // [Authorize]
        public async Task<ActionResult<Temperatura>> PostTemperatura(Temperatura temperatura)
        {
          if (_context.Temperaturas == null)
          {
              return Problem("Entity set 'AppDbContext.Temperaturas'  is null.");
          }
          temperatura.HorarioTemp = DateTime.Now;
            _context.Temperaturas.Add(temperatura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTemperatura", new { id = temperatura.IdTemperatura }, temperatura);
        }

        /// <summary>
        /// deleta os dados de uma temperatura em relação ao id
        /// </summary>

        // DELETE: api/Temperatura/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTemperatura(int id)
        {
            if (_context.Temperaturas == null)
            {
                return NotFound();
            }
            var temperatura = await _context.Temperaturas.FindAsync(id);
            if (temperatura == null)
            {
                return NotFound();
            }

            _context.Temperaturas.Remove(temperatura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TemperaturaExists(int id)
        {
            return (_context.Temperaturas?.Any(e => e.IdTemperatura == id)).GetValueOrDefault();
        }
    }
}
