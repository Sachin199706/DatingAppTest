using DatingApp.Data;
using DatingApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
  
    public class UserController : BaseAPIController
    {
        DataContext _dbContext;
        public UserController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetUsers() {
           var user=await _dbContext.UserDetails.ToListAsync();
            if (user == null) { 
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("{id}")]
       public IActionResult GetUesr(int id) {
           
         var user = _dbContext.UserDetails.Find(id);
            if(user==null) return NotFound();
            return Ok(user);
        }
    }
}
