using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TokenCreator
    {
        public static string CreateToken(
            Claim[] claims,
            DateTime expiringDate,
            string jwtSecret
            )
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(jwtSecret)
                            ),
                            SecurityAlgorithms.HmacSha256Signature
                        ),
                Issuer = "https://localhost:5001",
                Audience = "https://localhost:44339/api/"
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
