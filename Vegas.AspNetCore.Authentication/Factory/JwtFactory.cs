using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Vegas.AspNetCore.Authentication.Settings;

namespace Vegas.AspNetCore.Authentication.Factory
{
    internal class JwtFactory : IJwtFactory
    {
        private readonly IJwtSettings _jwtSettings;
        public JwtFactory(IJwtSettings jwtSettings) => _jwtSettings = jwtSettings;

        public string CreateToken(params string[] roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: roles.Select(role => new Claim(ClaimTypes.Role, role)),
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtSettings.GetExpireMinutesValue()),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return jwtToken;
        }
    }
}
