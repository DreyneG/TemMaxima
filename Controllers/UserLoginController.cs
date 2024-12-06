using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_TEMPERATURA_MAXIMA.Context;
using API_TEMPERATURA_MAXIMA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace API_TEMPERATURA_MAXIMA.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserLoginController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    // private readonly RoleManager<IdentityRole> _roleManeger;
    public UserLoginController(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration, AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        // _roleManeger = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    /// <summary>
        /// cria um usuário 
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
    public async Task<ActionResult<UserToken>> CreateUser([FromBody]
UserInfo model) 
    {
        var funcionarioCad = _context.Funcionarios.FirstOrDefault(e => e.email
        == model.email && e.cpf == model.cpf);
        if (funcionarioCad != null)
        {
            model.IdFuncionario = funcionarioCad.IdFuncionario;

           
            var user = new ApplicationUser
            {
              
                UserName = model.email,
                Email = model.email,
                Cpf = model.cpf,
                
                
            };
            var result = await _userManager.CreateAsync(user,model.password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Basic");
                var roles = await _userManager.GetRolesAsync(user);
                return BuildToken(model, roles);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos");
            }
        }
        else
        { return BadRequest("Funcionario não cadastrado"); }
    }

    /// <summary>
        /// retorna os usuários logados
        /// </summary>
        /// <remarks>
        /// {
        ///  "email": "string",
        ///  "password": "string",
        ///  "cpf": 0
        ///}
        /// </remarks>
        /// <response code="200">Sucesso no retorno dos dados</response>

    [HttpGet("Check")]
    public async Task<ActionResult<String>> CheckUser(String Cpf, string
    Email)
    {
        var FuncionarioCad = _context.Funcionarios.FirstOrDefault(e => e.email
        == Email && e.cpf == Cpf);
        var userExiste = _context.Users.FirstOrDefault(u => u.Email ==
        Email && u.Cpf == Cpf);
        if (userExiste != null)
        { return BadRequest("Usuario já cadastrado"); }
        else
        {
            if (FuncionarioCad != null)
            {

                return "OK";
            }
            else
            { return BadRequest("Paciente não cadastrado"); }
        }
        
    }
    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo
    userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.email, userInfo.password,isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(userInfo.email);
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.email == userInfo.email);
            userInfo.IdFuncionario = funcionario.IdFuncionario;
            var roles = await _userManager.GetRolesAsync(user);
            return BuildToken(userInfo, roles);
        }
        
        else
        {
            ModelState.AddModelError(string.Empty, "login inválido.");
            return BadRequest(ModelState);
        }
    }
    private UserToken BuildToken(UserInfo userInfo, IList<string>
    userRoles)
    {
        var claims = new List<Claim>
{
new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.email),
new Claim("meuValor", "oque voce quiser"),
new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
new Claim(ClaimTypes.NameIdentifier, userInfo.IdFuncionario.ToString())

};
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        var key = new
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
        var creds = new SigningCredentials(key,
        SecurityAlgorithms.HmacSha256);
        // tempo de expiração do token: 1 hora
        var expiration = DateTime.UtcNow.AddHours(1);
        JwtSecurityToken token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: expiration,
        signingCredentials: creds);

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Roles = userRoles
        };
    }
}
