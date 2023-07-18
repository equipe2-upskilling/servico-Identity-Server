using Grupo2_Identity_Server.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Grupo2_Identity_Server.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly IConfiguration _configuration;

        public AuthorizationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signManager = signinManager;
            _configuration = configuration;

        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsersDto usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var user = new IdentityUser
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, usuario.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _signManager.SignInAsync(user, false);

            return Ok(GeraToken(usuario));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsersDto usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var result = await _signManager.PasswordSignInAsync(usuario.Email, usuario.Password,
                                                                isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(GeraToken(usuario));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Invalido...");
                return BadRequest(ModelState);
            }
        }

        private TokenDto GeraToken(UsersDto usuarioDto)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuarioDto.Email),
                new Claim("AnimaUpSkilling", "grupo2"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracao = _configuration["TokenConfigurations:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfigurations:Issuer"],
                audience: _configuration["TokenConfigurations:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credenciais
                );

            return new TokenDto()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}
