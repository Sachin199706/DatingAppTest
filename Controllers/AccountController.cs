using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers
{
    public class AccountController : BaseAPIController
    {
      public readonly DataContext _dbContext;
        public readonly ITokenService _tokenService;
        public readonly IUserRepository _userRepository;    
        public AccountController(IUserRepository userRepository,ITokenService token, DataContext dbContext)
        {
            _userRepository = userRepository;
            _tokenService = token;
            _dbContext = dbContext;
        }
        [HttpPost("register")] //Account/register
        public async Task<ActionResult<TokenDto>> Register(RegisterDto register)
        {

            if (!await UserExits(register.username))
            {
                using var hashCode = new HMACSHA512();

                UserDetails User = new UserDetails
                {
                    UserName = register.username,
                    istrPasswordHash = hashCode.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
                    istrPasswordSalt = hashCode.Key,
                    City=register.city,
                    Country=register.counry,
                    Gender = register.gender,
                    KnownAs = register.KnowsAs,
                    DateofBerth = DateOnly.Parse(register.dateOfBirth)
                    
                };

                _dbContext.UserDetails.Add(User);
                await _dbContext.SaveChangesAsync();

                return new TokenDto
                {
                    UserName = User.UserName,
                    Token = _tokenService.CreateToken(User),
                    photoUrl = User.Photos.FirstOrDefault(x => x.IsMain)?.URL,
                    KnowAS = User.KnownAs
                };
                
            }
            else
            {
                return BadRequest("User Allready Register.");
            }
        }
        [HttpPost("Login")]//Account/Login
        public async Task<ActionResult<TokenDto>> Login(LogingDto logingDto)
        {

            UserDetails User = await _dbContext.UserDetails.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == logingDto.UserName.ToLower());
                //await _userRepository.GetUserByUserNameAsync(logingDto.UserName);
            if (User == null) return Unauthorized("Invalid User Name");
            using var hashCode = new HMACSHA512(User.istrPasswordSalt);
            byte[] CoumpeHashCode = hashCode.ComputeHash(Encoding.UTF8.GetBytes(logingDto.Password));
            for (int i = 0, ilen = CoumpeHashCode.Length; i < ilen; i++)
            {
                if (CoumpeHashCode[i] != User.istrPasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new TokenDto
            {
                UserName = User.UserName,
                Token = _tokenService.CreateToken(User),
                photoUrl = User.Photos.FirstOrDefault(x=>x.IsMain)?.URL,
                KnowAS=User.KnownAs
            };
         
           
        }
        public async Task<bool> UserExits(string astrUserName)
        {
           var user=  await _userRepository.GetUserByUserNameAsync(astrUserName);
            if (user == null) return false;
            return true;
        }

    }
}
