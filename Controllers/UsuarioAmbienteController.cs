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

        // GET: api/UsuarioAmbiente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioAmbiente>> GetUsuarioAmbiente(int id)
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

            return usuarioAmbiente;
        }

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
