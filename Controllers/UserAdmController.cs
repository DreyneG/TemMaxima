using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_TEMPERATURA_MAXIMA.Context;
using API_TEMPERATURA_MAXIMA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.IdentityModel.Tokens;
namespace API_TEMPERATURA_MAXIMA.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserAdmController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    public UserAdmController(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    /// <summary>
        /// insere um novo adiministrador no banco de dados
        /// </summary>
        /// <remarks>
        /// {
        ///  "email": "string",
        ///  "password": "string",
        ///  "cpf": 0
        ///}
        /// </remarks>
        /// <response code="200">Sucesso no upload dos dados</response>


    [HttpPost("Criar")]
public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
{
    var funcionario = _context.Funcionarios.FirstOrDefault(e => e.email == model.email && e.cpf == model.cpf);

    if (funcionario != null)
    {
        model.IdFuncionario = funcionario.IdFuncionario;
    }
    else
    {
        return BadRequest("Funcionário não encontrado no sistema.");
    }

    var user = new ApplicationUser
    {
        UserName = model.email,
        Email = model.email,
        Cpf = model.cpf
    };
    
    var result = await _userManager.CreateAsync(user, model.password);

    if (result.Succeeded)
    {
        await _userManager.AddToRoleAsync(user, "Admin");
        var roles = await _userManager.GetRolesAsync(user);
        return BuildToken(model, roles);
    }
    else
    {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest($"Erro ao criar usuário: {errors}");
    }
}

private UserToken BuildToken(UserInfo userInfo, IList<string> userRoles)
{
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, userInfo.IdFuncionario.ToString()),
        new Claim("IdFuncionario", userInfo.IdFuncionario.ToString())
    };

    foreach (var userRole in userRoles)
    {
        claims.Add(new Claim(ClaimTypes.Role, userRole));
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expiration = DateTime.UtcNow.AddHours(1);

    JwtSecurityToken token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: expiration,
        signingCredentials: creds);

    return new UserToken
    {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = expiration,
        Roles = userRoles
    };
}}