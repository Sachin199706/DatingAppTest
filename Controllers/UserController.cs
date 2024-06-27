using DatingApp.Data;
using DatingApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        DataContext _dbContext;
        public UserController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
           var user=await _dbContext.User.ToListAsync();
            if (user == null) { 
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("{id}")]
       public IActionResult GetUesr(int id) {
           
         var user = _dbContext.User.Find(id);
            if(user==null) return NotFound();
            return Ok(user);
        }
    }
}
