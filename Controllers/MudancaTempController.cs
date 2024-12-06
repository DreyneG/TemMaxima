using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API_TEMPERATURA_MAXIMA.Models;
using API_TEMPERATURA_MAXIMA.Context;

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

        /// <summary>
        /// retorna as mudanças temperaturas registrados no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idMudancaTemp": 0,
        ///    "temperatura_alterada": 0,
        ///    "temperatura": 0,
        ///    "nomeAmbiente": "string",
        ///    "idAmbiente": 0,
        ///    "nomeUsuario": "string",
        ///    "idFuncionario": 0,
        ///    "horarioAlteracao": "string",
        ///    "dataAlteracao": "2024-10-10"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/MudancaTemp
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MudancaTemp>>> GetMudancaTemps()
        {
            var funcionarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(funcionarioIdClaim) || !int.TryParse(funcionarioIdClaim, out var IdFuncionario))
            {
                return Unauthorized("Funcionário ID não encontrado ou inválido no token.");
            }

            var funcionario = await _context.Funcionarios.FindAsync(IdFuncionario);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            // Obter os IDs dos ambientes que o funcionário tem acesso
            var ambientesPermitidos = await _context.UsuarioAmbientes
                .Where(ua => ua.IdFuncionario == IdFuncionario)
                .Select(ua => ua.IdAmbiente)
                .ToListAsync();

            if (!ambientesPermitidos.Any())
            {
                return NotFound("Nenhum ambiente associado encontrado para o funcionário.");
            }

            // Buscar apenas as mudanças de temperatura nos ambientes permitidos
            var mudancas = await _context.MudancaTemps
                .Where(mt => ambientesPermitidos.Contains(mt.IdAmbiente))
                .ToListAsync();

            return Ok(mudancas);
        }

        /// <summary>
        /// retorna a mudança temperatura registrada no banco de dados em relação ao id
        /// </summary>
        /// <summary>
        /// retorna as mudanças temperaturas registrados no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idMudancaTemp": 0,
        ///    "temperatura_alterada": 0,
        ///    "temperatura": 0,
        ///    "nomeAmbiente": "string",
        ///    "idAmbiente": 0,
        ///    "nomeUsuario": "string",
        ///    "idFuncionario": 0,
        ///    "horarioAlteracao": "string",
        ///    "dataAlteracao": "2024-10-10"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

        // GET: api/MudancaTemp/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MudancaTemp>> GetMudancaTemp(int id)
        {
            var funcionarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(funcionarioIdClaim) || !int.TryParse(funcionarioIdClaim, out var IdFuncionario))
            {
                return Unauthorized("Funcionário ID não encontrado ou inválido no token.");
            }

            var funcionario = await _context.Funcionarios.FindAsync(IdFuncionario);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            var mudancaTemp = await _context.MudancaTemps .OrderByDescending(m => m.IdMudancaTemp).Where(t => t.IdAmbiente == id).FirstOrDefaultAsync();

            if (mudancaTemp == null)
            {
                return NotFound("Mudança de temperatura não encontrada.");
            }

            if (!VerificarAcesso(funcionario.IdFuncionario, mudancaTemp.IdAmbiente))
            {
                return Forbid("Acesso negado ao ambiente.");
            }

            return Ok(mudancaTemp);
        }

        /// <summary>
        /// insere dados de mudança temperatura no banco de dados
        /// </summary>
        /// <summary>
        /// retorna as mudanças temperaturas registrados no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///    "idMudancaTemp": 0,
        ///    "temperatura_alterada": 0,
        ///    "temperatura": 0,
        ///    "nomeAmbiente": "string",
        ///    "idAmbiente": 0,
        ///    "nomeUsuario": "string",
        ///    "idFuncionario": 0,
        ///    "horarioAlteracao": "string",
        ///    "dataAlteracao": "2024-10-10"
        ///  }
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>

        // POST: api/MudancaTemp
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MudancaTemp>> PostMudancaTemp(MudancaTemp mudancaTemp)
        {
            var funcionarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(funcionarioIdClaim) || !int.TryParse(funcionarioIdClaim, out var IdFuncionario))
            {
                return Unauthorized("Funcionário ID não encontrado ou inválido no token.");
            }

            var funcionario = await _context.Funcionarios.FindAsync(IdFuncionario);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            if (!VerificarAcesso(funcionario.IdFuncionario, mudancaTemp.IdAmbiente))
            {
                return Forbid("Acesso negado ao ambiente.");
            }

            mudancaTemp.HorarioMudanca = DateTime.Now;

            _context.MudancaTemps.Add(mudancaTemp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMudancaTemp", new { id = mudancaTemp.IdMudancaTemp }, mudancaTemp);
        }

        private bool VerificarAcesso(int idFuncionario, int idAmbiente)
        {
            return _context.UsuarioAmbientes
                .Any(fa => fa.IdFuncionario == idFuncionario && fa.IdAmbiente == idAmbiente);
        }
    }
}
