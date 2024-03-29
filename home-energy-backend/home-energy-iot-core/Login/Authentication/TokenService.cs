﻿using home_energy_iot_entities.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace home_energy_iot_core.Login.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private static JwtSecurityTokenHandler tokenHandler = new();

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtKeyBytes = Encoding.ASCII.GetBytes(_configuration["SecretJwtKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(525960),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}