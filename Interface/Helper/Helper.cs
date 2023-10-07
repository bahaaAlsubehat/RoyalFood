using Interface.DTO;
using Interface.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Helper
{
    public class Helper
    {
        private readonly IConfiguration _config;

        public Helper(IConfiguration config) 
        {
            _config = config;
        
        }
        public string GenerateJwtToken(string email, string role, int userid, bool isactive)
        {

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var SigningCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub, email));
            claims.Add(new(ClaimTypes.Role, role));
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(12),
                SigningCreds
            );
            return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
        
        }

        public bool ValidateJWTtoken(string tokenString, out LogoutResponse response)
        {
            String toke = "Bearer " + tokenString;
            var jwtEncodedString = toke.Substring(7);
            //var handler = new JwtSecurityTokenHandler();
            //var tokendata = handler.ReadJwtToken(toke) as JwtSecurityToken;
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);

            DateTime dateTime = DateTime.UtcNow;
            DateTime expires = token.ValidTo;
            if (dateTime < expires)
            {
                LogoutResponse tempresponse = new LogoutResponse();
                tempresponse.userid = Int32.Parse((token.Claims.First(c => c.Type == "UserId").Value.ToString()));
                tempresponse.loginid = Int32.Parse(token.Claims.First(c => c.Type == "LoginId").Value.ToString());
                tempresponse.cartUser = null;
                response = tempresponse;
                return true;
            }
            response = null;
            return false;


        }


        public string GenerateSHA384String(string inputString)
        {
            SHA384 sha348 = SHA384.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha348.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
        public string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}
