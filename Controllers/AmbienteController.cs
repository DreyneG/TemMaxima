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

namespace API_TEMPERATURA_MAXIMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AmbienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AmbienteController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// retorna os ambientes registrados no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idAmbiente": 0,
        ///    "nomeAmbiente": "string"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>
    

        // GET: api/Ambiente
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ambiente>>> GetAmbientes()
        {
          if (_context.Ambientes == null)
          {
              return NotFound();
          }
            return await _context.Ambientes.ToListAsync();
            
        }

        /// <summary>
        /// retorna um ambiente registrado no banco de dados em relação ao id inserido
        /// </summary>
        /// <remarks>
        /// {
        ///    "idAmbiente": 0,
        ///    "nomeAmbiente": "string"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>


        // GET: api/Ambiente/5
        [HttpGet("{id}")]
        [Authorize]
      
        public async Task<ActionResult<Ambiente>> GetAmbiente(int id)
        {
          if (_context.Ambientes == null)
          {
              return NotFound();
          }
            var ambiente = await _context.Ambientes.FindAsync(id);

            if (ambiente == null)
            {
                return NotFound();
            }

            return ambiente;
        }

        /// <summary>
        /// faz update de um ambiente em relação ao id
        /// </summary>
        /// <remarks>
        /// {
        ///    "idAmbiente": 0,
        ///    "nomeAmbiente": "string"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no update dos dados</response>


        // PUT: api/Ambiente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
          [Authorize("Admin")]
        public async Task<IActionResult> PutAmbiente(int id, Ambiente ambiente)
        {
            if (id != ambiente.IdAmbiente)
            {
                return BadRequest();
            }

            _context.Entry(ambiente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmbienteExists(id))
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
        /// insere um novo ambiente no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idAmbiente": 0,
        ///    "nomeAmbiente": "string"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>

        // POST: api/Ambiente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
          [Authorize("Admin")]
        public async Task<ActionResult<Ambiente>> PostAmbiente(Ambiente ambiente)
        {
          if (_context.Ambientes == null)
          {
              return Problem("Entity set 'AppDbContext.Ambientes'  is null.");
          }
            _context.Ambientes.Add(ambiente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmbiente", new { id = ambiente.IdAmbiente }, ambiente);
        }

        /// <summary>
        /// deleta um ambiente do banco de dados
        /// </summary>

        // DELETE: api/Ambiente/5
        [HttpDelete("{id}")]
          [Authorize("Admin")]
        public async Task<IActionResult> DeleteAmbiente(int id)
        {
            if (_context.Ambientes == null)
            {
                return NotFound();
            }
            var ambiente = await _context.Ambientes.FindAsync(id);
            if (ambiente == null)
            {
                return NotFound();
            }

            _context.Ambientes.Remove(ambiente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmbienteExists(int id)
        {
            return (_context.Ambientes?.Any(e => e.IdAmbiente == id)).GetValueOrDefault();
        }
    }
}
