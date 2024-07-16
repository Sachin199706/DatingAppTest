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
        public DataContext _dbContext { get; set; }
        public ITokenService _tokenService { get; set; }
        public AccountController(DataContext dataContext,ITokenService token)
        {
            _dbContext = dataContext;
            _tokenService = token;
        }
        [HttpPost("register")] //Account/register
        public async Task<ActionResult<UserDetails>> Register(RegisterDto register)
        {
            if (!await UserExits(register.UserName))
            {
                using var hashCode = new HMACSHA512();

                UserDetails User = new UserDetails
                {
                    UserName = register.UserName,
                    istrPasswordHash = hashCode.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                    istrPasswordSalt = hashCode.Key
                };

                _dbContext.UserDetails.Add(User);
                await _dbContext.SaveChangesAsync();

                return User;
            }
            else
            {
                return BadRequest("User Allready Register.");
            }
        }
        [HttpPost("Login")]//Account/Login
        public async Task<ActionResult<TokenDto>> Login(LogingDto logingDto)
        {
            UserDetails User = await _dbContext.UserDetails.FirstOrDefaultAsync(x => x.UserName.ToLower() == logingDto.UserName);
            if (User == null) return Unauthorized("Invalid User Name");
            using var hashCode = new HMACSHA512(User.istrPasswordSalt);
            byte[] CoumpeHashCode = hashCode.ComputeHash(Encoding.UTF8.GetBytes(logingDto.Password));
            for (int i=0,ilen= CoumpeHashCode.Length; i < ilen; i++)
            {
                if (CoumpeHashCode[i] != User.istrPasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new TokenDto {
                UserName = User.UserName,
                Token = _tokenService.CreateToken(User)
            };
           
        }
        public async Task<bool> UserExits(string astrUserName)
        {
            return await _dbContext.UserDetails.AnyAsync(x => x.UserName.ToLower() == astrUserName.ToLower());
        }

    }
}
