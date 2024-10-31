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


        // GET: api/Estado_ar/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Estado_ar>>> GetEstado_ar(int id, [FromQuery] Estado_ar estado_Ar)
        {

            int i = 0;
            int a = 0;

            string query = "SELECT temperatura FROM \"Temperaturas\" ORDER BY \"IdTemperatura\" DESC LIMIT 1";
            string query3 = "SELECT IdAmbiente FROM \"Temperaturas\" ORDER BY \"IdTemperatura\" DESC LIMIT 1";
            string query2 = "SELECT temperatura_alterada FROM \"MudancaTemps\" ORDER BY \"IdMudancaTemp\" DESC LIMIT 1";
            string conexao = "Server=localhost;Port=5432;Database=TemperaturaMaxima;User Id=postgres;Password=senai901;SearchPath=public;";

            float temp_Atual = 0;
            int temp_alterada = 0;


            using (var connection = new NpgsqlConnection(conexao))
            {
                connection.Open();

                // Executando ambas as consultas na mesma conexão
                using (var command = new NpgsqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        temp_Atual = Convert.ToSingle(result);
                    }
                }

                using (var command = new NpgsqlCommand(query2, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        temp_alterada = Convert.ToInt32(result);
                    }
                }
                using (var command = new NpgsqlCommand(query3, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        a = Convert.ToInt32(result);
                    }
                }
            }

            // Ajustando a lógica da comparação de temperaturas para clareza
            if (temp_Atual >= temp_alterada + 1)
            {
                i = 0; // Ar-condicionado está desligado
            }
            else if (temp_Atual == temp_alterada)
            {
                i = 1;
            }
            else if (temp_Atual <= temp_alterada - 1)
            {
                i = 1; // Ar-condicionado está ligado
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
            var estado_ar = await _context.Estado_ar.Where(e => e.IdAmbiente == id).ToListAsync();

            if (estado_ar == null || estado_ar.Count == 0)
            {
                return NotFound();
            }

            return estado_ar;
        }

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
