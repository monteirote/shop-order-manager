using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrderManager.Database.Models;

namespace OrderManager.Services
{
    public interface ITokenService
    {
        string GenerateToken (User user);
    }

    public class TokenService : ITokenService
    {
        private readonly string _secret;
        private readonly int _expirationMinutes;

        public TokenService (IConfiguration configuration)
        {
            // O segredo e tempo de expiração podem ser configurados no appsettings.json
            _secret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret");
            _expirationMinutes = int.TryParse(configuration["Jwt:ExpirationMinutes"], out var min) ? min : 60;
        }

        public string GenerateToken (User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "OrderManager",
                audience: "OrderManager",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }       
}
