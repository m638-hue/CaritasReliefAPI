using System;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Security.Cryptography;
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
            var passHashed = ComputeSHA256(creds.password);
            var login = context.Logins.Where(x => 
                x.usuario == creds.username && 
                x.contrasena ==  passHashed)
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

        public static string ComputeSHA256(string s)
        {
            string hash = String.Empty;

            // Initialize a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the given string
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

                // Convert the byte array to string format
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }

            return hash;
        }
    }
}