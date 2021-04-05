using EcommerceMVC.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceMVC.Auth
{
    public class AuthManager : IJwtAuthManager
    {
        public string Authenticate(string name, string password)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtValues.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,name),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,name)
            };

            var tokenDescription = new JwtSecurityToken(
                JwtValues.Issuer,
                JwtValues.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );
            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(tokenDescription),
                expiration = tokenDescription.ValidTo
            };

            return results.token;

        }
    }
}
