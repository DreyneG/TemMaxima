using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TEMPERATURA_MAXIMA.Context;
using API_TEMPERATURA_MAXIMA.Models;

namespace API_TEMPERATURA_MAXIMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAmbienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioAmbienteController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// retorna os usuários que estão associados a um ambiente
        /// </summary>
        /// <remarks>
        /// {
        ///    "idUsuarioAmbiente": 0,
        ///    "idAmbiente": 0,
        ///    "idFuncionario": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/UsuarioAmbiente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioAmbiente>>> GetUsuarioAmbientes()
        {
          if (_context.UsuarioAmbientes == null)
          {
              return NotFound();
          }
            return await _context.UsuarioAmbientes.ToListAsync();
        }

        /// <summary>
        /// retorna os usuários que estão associados a um ambiente baseado no id
        /// </summary>
        /// <remarks>
        /// {
        ///    "idUsuarioAmbiente": 0,
        ///    "idAmbiente": 0,
        ///    "idFuncionario": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/UsuarioAmbiente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UsuarioAmbiente>>> GetUsuarioAmbiente(int id)
        {
          if (_context.UsuarioAmbientes == null)
          {
              return NotFound();
          }
            var usuarioAmbiente = await _context.UsuarioAmbientes.Where(e => e.IdAmbiente == id).ToListAsync();

            if (usuarioAmbiente == null)
            {
                return NotFound();
            }

            return usuarioAmbiente;
        }

        /// <summary>
        /// atualiza os usuários que estão associados a um ambiente
        /// </summary>
        /// <remarks>
        /// {
        ///    "idUsuarioAmbiente": 0,
        ///    "idAmbiente": 0,
        ///    "idFuncionario": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no update dos dados</response>

        // PUT: api/UsuarioAmbiente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioAmbiente(int id, UsuarioAmbiente usuarioAmbiente)
        {
            if (id != usuarioAmbiente.IdUsuarioAmbiente)
            {
                return BadRequest();
            }

            _context.Entry(usuarioAmbiente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioAmbienteExists(id))
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
        /// insere usuários em um ambiente
        /// </summary>
        /// <remarks>
        /// {
        ///    "idUsuarioAmbiente": 0,
        ///    "idAmbiente": 0,
        ///    "idFuncionario": 0
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>

        // POST: api/UsuarioAmbiente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioAmbiente>> PostUsuarioAmbiente(UsuarioAmbiente usuarioAmbiente)
        {
          if (_context.UsuarioAmbientes == null)
          {
              return Problem("Entity set 'AppDbContext.UsuarioAmbientes'  is null.");
          }
            _context.UsuarioAmbientes.Add(usuarioAmbiente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarioAmbiente", new { id = usuarioAmbiente.IdUsuarioAmbiente }, usuarioAmbiente);
        }

        /// <summary>
        /// deleta os usuários que estão associados a um ambiente baseado no id
        /// </summary>

        // DELETE: api/UsuarioAmbiente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioAmbiente(int id)
        {
            if (_context.UsuarioAmbientes == null)
            {
                return NotFound();
            }
            var usuarioAmbiente = await _context.UsuarioAmbientes.FindAsync(id);
            if (usuarioAmbiente == null)
            {
                return NotFound();
            }

            _context.UsuarioAmbientes.Remove(usuarioAmbiente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioAmbienteExists(int id)
        {
            return (_context.UsuarioAmbientes?.Any(e => e.IdUsuarioAmbiente == id)).GetValueOrDefault();
        }
    }
}
