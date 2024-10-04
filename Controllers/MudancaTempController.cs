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
    public class MudancaTempController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MudancaTempController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MudancaTemp
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MudancaTemp>>> GetMudancaTemps()
        {
          if (_context.MudancaTemps == null)
          {
              return NotFound();
          }
            return await _context.MudancaTemps.ToListAsync();
        }

        // GET: api/MudancaTemp/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MudancaTemp>> GetMudancaTemp(int id)
        {
          if (_context.MudancaTemps == null)
          {
              return NotFound();
          }
            var mudancaTemp = await _context.MudancaTemps.FindAsync(id);

            if (mudancaTemp == null)
            {
                return NotFound();
            }

            return mudancaTemp;
        }

        // PUT: api/MudancaTemp/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutMudancaTemp(int id, MudancaTemp mudancaTemp)
        {
            if (id != mudancaTemp.IdMudancaTemp)
            {
                return BadRequest();
            }

            _context.Entry(mudancaTemp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MudancaTempExists(id))
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

        // POST: api/MudancaTemp
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MudancaTemp>> PostMudancaTemp(MudancaTemp mudancaTemp)
        {
          if (_context.MudancaTemps == null)
          {
              return Problem("Entity set 'AppDbContext.MudancaTemps'  is null.");
          }
            _context.MudancaTemps.Add(mudancaTemp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMudancaTemp", new { id = mudancaTemp.IdMudancaTemp }, mudancaTemp);
        }

        // DELETE: api/MudancaTemp/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMudancaTemp(int id)
        {
            if (_context.MudancaTemps == null)
            {
                return NotFound();
            }
            var mudancaTemp = await _context.MudancaTemps.FindAsync(id);
            if (mudancaTemp == null)
            {
                return NotFound();
            }

            _context.MudancaTemps.Remove(mudancaTemp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MudancaTempExists(int id)
        {
            return (_context.MudancaTemps?.Any(e => e.IdMudancaTemp == id)).GetValueOrDefault();
        }
    }
}
