using Microsoft.IdentityModel.Tokens;
using RealTimeChat.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeChat.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenToken(User user)
        {
            var settings = _config.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(settings["Key"]);
            var expiration = int.Parse(settings["ExpiryMinutes"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: settings["Issuer"],
                audience: settings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);

        }



    }
}
