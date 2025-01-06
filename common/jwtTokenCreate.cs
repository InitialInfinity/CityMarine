using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Context
{
    public class jwtTokenCreate: ControllerBase
    {
        private readonly IConfiguration _configuration;
        public jwtTokenCreate(IConfiguration configuration)
        {
           _configuration= configuration;
        }

        //Token Creation
        public JwtSecurityToken createToken(string? Id)
        {
            var now = DateTime.UtcNow;

            var authClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
;

            return token;
        }

        //Token Encryption
        public  string Encrypt(string source, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();

            byte[] byteBuff;

            try
            {
                desCryptoProvider.Key = Encoding.UTF8.GetBytes(key);
                desCryptoProvider.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
                byteBuff = Encoding.UTF8.GetBytes(source);

                string iv = Convert.ToBase64String(desCryptoProvider.IV);
                Console.WriteLine("iv: {0}", iv);

                string encoded =
                    Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));

                return encoded;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Token Decryption 
        public  string Decrypt(string encodedText, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();

            byte[] byteBuff;

            try
            {
                desCryptoProvider.Key = Encoding.UTF8.GetBytes(key);
                desCryptoProvider.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
                byteBuff = Convert.FromBase64String(encodedText);

                string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                return plaintext;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public string GenerateJwtToken(string comId, string serverValue, string comname,string loginId, string roleId, string roleName, string staffId, string baseAddress, string countryCode, string timezone, string currencyFormat, string currencySymbol,string st_staff_name, string baseaddresuser)
        {
            var claims = new[]
            {
            new Claim("com_id", comId),
            new Claim("Server_Value", serverValue),
            new Claim("loginId", loginId),
            new Claim("RoleId", roleId),
            new Claim("RoleName", roleName),
            new Claim("StaffId", staffId),
            new Claim("BaseAddress", baseAddress),
            new Claim("co_country_code", countryCode),
            new Claim("co_timezone", timezone),
            new Claim("cm_currency_format", currencyFormat),
            new Claim("cm_currencysymbol", currencySymbol),
            new Claim("baseaddresuser", baseaddresuser),
            new Claim("st_staff_name", st_staff_name),
            new Claim("comname", comname)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
