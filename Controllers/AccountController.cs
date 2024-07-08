using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers
{
    public class AccountController : BaseAPIController
    {
        public DataContext _dbContext;
       public AccountController(DataContext dataContext)
        {
            _dbContext = dataContext;
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
        public async Task<bool> UserExits(string astrUserName)
        {
            return await _dbContext.UserDetails.AnyAsync(x => x.UserName.ToLower() == astrUserName.ToLower()); 
        }

    }
}
