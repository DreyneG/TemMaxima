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

        // GET: api/Temperatura
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Temperatura>>> GetTemperaturas()
        {
          if (_context.Temperaturas == null)
          {
              return NotFound();
          }
            return await _context.Temperaturas.ToListAsync();
        }

        // GET: api/Temperatura/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Temperatura>> GetTemperatura(int id)
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

            return temperatura;
        }

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

        // POST: api/Temperatura
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Temperatura>> PostTemperatura(Temperatura temperatura)
        {
          if (_context.Temperaturas == null)
          {
              return Problem("Entity set 'AppDbContext.Temperaturas'  is null.");
          }
            _context.Temperaturas.Add(temperatura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTemperatura", new { id = temperatura.IdTemperatura }, temperatura);
        }

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
