using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reto.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JwtService _jwtService;

        public AuthController(ILogger<AuthController> logger, JwtService jwtService)
        {
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult auth(
            [FromBody] Credentials creds,
            [FromServices] SQLContext context)
        {
            var login = context.Logins.Where(x => 
                x.usuario == creds.username && 
                x.contrasena ==  creds.password)
                .FirstOrDefault();

            if (login != null)
            {
                var role = login.tipo == 2 ? Role.admin : Role.user;
                var token = _jwtService.GenerateToken(creds, role);
                object user;

                if (role == Role.admin)
                    user = context.Admins
                        .Where(a => a.idLogin == login.idLogin)
                        .Select(a => a.idAdmin);

                else
                    user = context.Recolectores
                        .Where(r => r.idLogin == login.idLogin)
                        .Select(r => r.idRecolector);

                if (user != null)
                    return Ok(new
                    {
                        token,
                        role,
                        user
                    });
            }

            return Unauthorized("Incorrect user or password");
        }
    }
}