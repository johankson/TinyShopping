using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TinyShopping.Api.Services
{
    public class TokenService
    {
        public TokenService(IConfiguration config)
        {
            Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["AuthToken"]));
        }

        public SymmetricSecurityKey Key { get; internal set; }

        public string GetUsername(string key) {
            var data = new JwtSecurityTokenHandler().ReadToken(key) as JwtSecurityToken;
            var userClaim = data.Claims.FirstOrDefault(d => d.Type == ClaimTypes.Name);
            if (userClaim != null)
                return userClaim.Value;
            return string.Empty;
        }

        public string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(10)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
