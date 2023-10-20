using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Reto
{
    public enum Role
    {
        admin,
        user
    }

    public class JwtService
    {
        string secretKey;
        string issuer;
        string audience; 

        public JwtService(string secretKey, string issuer, string audience)
        {
            this.secretKey = secretKey;
            this.issuer = issuer;
            this.audience = audience;
        }

        public string GenerateToken(Credentials credentials, Role role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, credentials.username),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("role", role.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}