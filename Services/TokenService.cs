using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Services
{
    public class TokenService:ITokenService
    {
        public IConfiguration _Configuration { get; set; }
        public TokenService(IConfiguration configuration) {
            _Configuration = configuration;
        }

        public string CreateToken(UserDetails user)
        {
            string Token = _Configuration["Tokenkey"] ?? throw new Exception("Can not access Tokenkey form appSettingJson");
            if (Token.Length < 64) throw new Exception("Your Token Key Needs to be longer");
            SymmetricSecurityKey key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Token));
            List<Claim> claims = new List<Claim>();
            Claim Test = new Claim(ClaimTypes.NameIdentifier,user.UserName); 
            claims.Add(Test);
            SigningCredentials signing = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires= DateTime.UtcNow.AddDays(7),
                SigningCredentials= signing

            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var Tokek=handler.CreateToken(TokenDescriptor); 
            return handler.WriteToken(Tokek);

        }
    }
}
