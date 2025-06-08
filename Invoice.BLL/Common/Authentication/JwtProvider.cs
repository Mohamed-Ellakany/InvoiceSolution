using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Invoice.BLL.Common.Authentication
{
    public class JwtProvider : IJwtProvider
    {

        public (string token, int expiresIn) GenerateToken(IdentityUser user)
        {
            Claim[] claims = [
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email!),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                ];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1uDZcf23nTqXgOIrvLwbW4qocRiueNl1"));


            var singingCredentials = new SigningCredentials (symmetricSecurityKey , SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : "InvoiceApp",
                audience: "InvoiceApp Users",
                claims :claims,
                expires: DateTime.UtcNow.AddMinutes(1000),
                signingCredentials : singingCredentials
                );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: 1000);
        }
    }
}
