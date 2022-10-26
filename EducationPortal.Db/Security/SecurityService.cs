using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EducationPortal.Db.Security
{
    public static class SecurityService
    {
        private const string Secret = "asdv234234^&%&^%&^hjsdfb2%%%";
        private const string Issuer = "http://mysite.com";
        private const string Audience = "http://myaudience.com";

        public static string GenerateToken(int userId)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateCurrentToken(string token, int userId)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var t = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = mySecurityKey
                }, out _);
                var userIdStr = t.Claims.First(x => x.Type == "UserId").Value;

                if (int.Parse(userIdStr) != userId)
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static int GetUserIdFromToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var t = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = mySecurityKey
                }, out _);
                var userIdStr = t.Claims.First(x => x.Type == "UserId").Value;


                return int.Parse(userIdStr);
            }
            catch
            {
                return -1;
            }
        }



    }
}