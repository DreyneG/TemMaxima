using System.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TEMPERATURA_MAXIMA.Context;
using API_TEMPERATURA_MAXIMA.Models;
using Npgsql;
using System.Xml;
using Newtonsoft.Json;

namespace API_TEMPERATURA_MAXIMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Estado_arController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Estado_arController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// retorna os dados sobre os Estados do ar
        /// </summary>
        /// <remarks>
        /// {
        ///    "idEstado_ar": 0,
        ///    "estado": 0,
        ///    "idAmbiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/Estado_ar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado_ar>>> GetEstado_ar()
        {

            if (_context.Estado_ar == null)
            {
                return NotFound();
            }

            return await _context.Estado_ar.ToListAsync();

        }

        /// <summary>
        /// retorna os dados sobre os Estados do ar baseado no id
        /// </summary>
        /// <remarks>
        /// {
        ///    "idEstado_ar": 0,
        ///    "estado": 0,
        ///    "idAmbiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>


        // GET: api/Estado_ar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estado_ar>> GetEstado_ar(int id)
        {
            int i = 0;
            int a = id;

            var temp_Atual = await _context.Temperaturas.Where(t => t.IdAmbiente == id).OrderByDescending(t => t.IdTemperatura).Select(t => t.temperatura).FirstOrDefaultAsync();

            // Obter o último IdAmbiente da tabela Temperaturas
            var ambienteId = await _context.Temperaturas
                                           .OrderByDescending(t => t.IdTemperatura)
                                           .Select(t => t.IdAmbiente).FirstOrDefaultAsync();

            // Obter a última temperatura alterada da tabela MudancaTemps
            var temp_alterada = await _context.MudancaTemps.Where(t => t.IdAmbiente == id)
                                              .OrderByDescending(m => m.IdMudancaTemp)
                                              .Select(m => m.temperatura_alterada)
                                              .FirstOrDefaultAsync();

            // Ajustando a lógica da comparação de temperaturas para clareza
            if (temp_Atual <= temp_alterada + 1)
            {

                i = 0;
                // return BadRequest(); // Ar-condicionado está desligado
            }
            else if (temp_Atual == temp_alterada)
            {
                i = 1;
                // return BadRequest();
            }
            else if (temp_Atual >= temp_alterada - 1)
            {
                i = 1;
                // return BadRequest(); // Ar-condicionado está ligado
            }


            // Criando o objeto Estado_ar a partir do JSON
            var e = "{\"idEstado_ar\":0, \"estado\":" + i + ", \"IdAmbiente\":" + a + "}";
            var estado = JsonConvert.DeserializeObject<Estado_ar>(e);
            // Verifica se a deserialização foi bem-sucedida
            if (estado == null)
            {
                return BadRequest("Erro ao processar o estado do ar-condicionado.");
            }

            // Postando o estado
            await PostEstado_ar(estado);


            if (_context.Estado_ar == null)
            {
                return NotFound();
            }
            var estado_ar = await _context.Estado_ar.OrderDescending().Where(e => e.IdAmbiente == id).FirstOrDefaultAsync();

            if (estado_ar == null)
            {
                return NotFound();
            }

            return estado_ar;
        }

        /// <summary>
        /// atualiza os dados sobre os Estados do ar
        /// </summary>
        /// <remarks>
        /// {
        ///    "idEstado_ar": 0,
        ///    "estado": 0,
        ///    "idAmbiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no update dos dados</response>

        // PUT: api/Estado_ar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado_ar(int id, Estado_ar estado_ar)
        {
            if (id != estado_ar.IdEstado_ar)
            {
                return BadRequest();
            }

            _context.Entry(estado_ar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Estado_arExists(id))
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
        /// insere dados sobre os Estados do ar
        /// </summary>
        /// <remarks>
        /// {
        ///    "idEstado_ar": 0,
        ///    "estado": 0,
        ///    "idAmbiente": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>

        // POST: api/Estado_ar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estado_ar>> PostEstado_ar(Estado_ar estado_ar)
        {

            if (_context.Estado_ar == null)
            {
                return Problem("Entity set 'AppDbContext.Estado_ar'  is null.");
            }
            _context.Estado_ar.Add(estado_ar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstado_ar", new { id = estado_ar.IdEstado_ar }, estado_ar);
            // return BadRequest(e);
        }

        /// <summary>
        /// deleta dados sobre os Estados do ar baseado no id
        /// </summary>

        // DELETE: api/Estado_ar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstado_ar(int id)
        {
            if (_context.Estado_ar == null)
            {
                return NotFound();
            }
            var estado_ar = await _context.Estado_ar.FindAsync(id);
            if (estado_ar == null)
            {
                return NotFound();
            }

            _context.Estado_ar.Remove(estado_ar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Estado_arExists(int id)
        {
            return (_context.Estado_ar?.Any(e => e.IdEstado_ar == id)).GetValueOrDefault();
        }

    }
}
