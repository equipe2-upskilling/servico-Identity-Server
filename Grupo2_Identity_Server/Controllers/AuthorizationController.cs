using Grupo2_Identity_Server.Dtos;
using Grupo2_Identity_Server.Entities;
using Grupo2_Identity_Server.Interfaces;
using Grupo2_Identity_Server.ModelViews;
using Grupo2_Identity_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grupo2_Identity_Server.Controllers
{
    [ApiController]
    [Route("/Identity")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IToken _token;
        private readonly ICrypto _crypto;
        private readonly ILoginRepository _loginRepository;

        public AuthorizationController(IToken token, ICrypto crypto, ILoginRepository loginRepository)
        {
            _token = token;
            _crypto = crypto;
            _loginRepository = loginRepository;
        }

        [HttpPost("/register")]
        public async Task<ActionResult> RegisterUser([FromBody] LoginDto usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var salt = _crypto.GetSalt();
            var user = new User
            {
                Email = usuario.Username,
                Password = _crypto.Encrypt(usuario.Password,salt),
                Salt = salt

            };

            await _loginRepository.AddAsync(user);


            return Ok(new HttpReturn { Message = "Administrador criado com sucesso" });
        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login([FromBody] LoginDto usuario)
        {
            var userList = await _loginRepository.GetByEmailAsync(usuario.Username);
            if (userList == null) return NotFound(new HttpReturn { Message = "Não encontrado." });

            var user = userList.First();
            var pass = _crypto.Encrypt(usuario.Password, user.Salt);

            if (user.Password != pass) return BadRequest(new HttpReturn { Message = "Credenciais inválidas" });

            var logged = SimpleLogged.Build(user);

            return Ok(new Authenticated
            {
                SimpleLogged = logged,
                Token = new AccessService(_token).BuildToken(user)
            });
            
        }
        [HttpPost("/refresh-token")]
        public IActionResult RefreshToken()
        {
            var user = GetToken();
            if (user == null) return Forbid();

            var logged = SimpleLogged.Build(user);
            return Ok(new Authenticated
            {
                SimpleLogged = logged,
                Token = new AccessService(_token).BuildToken(user)
            });
        }

        [HttpHead("/valid-token")]
        public IActionResult ValidToker()
        {
            return GetToken() != null ? Ok() : Forbid();
        }

        private User GetToken()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length);
                var user = new AccessService(_token).TokenAccess(token);
                return user;
            }

            return null;
        }

    }
}
