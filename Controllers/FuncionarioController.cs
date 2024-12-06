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
    // [Authorize("Admin")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// retorna os funcionarios registrados no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idFuncionario": 0,
        ///    "nome_usuario": "string",
        ///    "email": "string",
        ///    "password": "string",
        ///    "cpf": 0,
        ///    "type_adm": true
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>


        // GET: api/Funcionario
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
          if (_context.Funcionarios == null)
          {
              return NotFound();
          }
            return await _context.Funcionarios.ToListAsync();
        }

        /// <summary>
        /// retorna o funcionario registrado no banco de dados em relação ao id
        /// </summary>
        /// <remarks>
        /// {
        ///    "idFuncionario": 0,
        ///    "nome_usuario": "string",
        ///    "email": "string",
        ///    "password": "string",
        ///    "cpf": 0,
        ///    "type_adm": true
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>


        // GET: api/Funcionario/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
          if (_context.Funcionarios == null)
          {
              return NotFound();
          }
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return funcionario;
        }

        /// <summary>
        /// faz um update dos dados de um funcionário em relação ao id
        /// </summary>
        /// <remarks>
        /// {
        ///    "idFuncionario": 0,
        ///    "nome_usuario": "string",
        ///    "email": "string",
        ///    "password": "string",
        ///    "cpf": 0,
        ///    "type_adm": true
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no update dos dados</response>

        // PUT: api/Funcionario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
          [Authorize("Admin")]
        public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionario)
        {
            if (id != funcionario.IdFuncionario)
            {
                return BadRequest();
            }

            _context.Entry(funcionario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(id))
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
        /// insere um funcionário no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idFuncionario": 0,
        ///    "nome_usuario": "string",
        ///    "email": "string",
        ///    "password": "string",
        ///    "cpf": 0,
        ///    "type_adm": true
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>

        // POST: api/Funcionario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //   [Authorize("Admin")]
        public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
        {
          if (_context.Funcionarios == null)
          {
              return Problem("Entity set 'AppDbContext.Funcionarios'  is null.");
          }
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFuncionario", new { id = funcionario.IdFuncionario }, funcionario);
        }

        /// <summary>
        /// deleta um funcionário do banco de dados em relação ao id
        /// </summary>

        // DELETE: api/Funcionario/5
        [HttpDelete("{id}")]
          [Authorize("Admin")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            if (_context.Funcionarios == null)
            {
                return NotFound();
            }
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionarioExists(int id)
        {
            return (_context.Funcionarios?.Any(e => e.IdFuncionario == id)).GetValueOrDefault();
        }
    }
}
