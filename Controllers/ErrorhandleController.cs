using DatingApp.Data;
using DatingApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DatingApp.Controllers
{
    public class ErrorhandleController : BaseAPIController
    {
        public DataContext _Context { get; set; }
        public ErrorhandleController(DataContext dbContext)
        {
            _Context = dbContext;
        }
        [Authorize]
        [HttpGet("auth")]//Errorhandle/auth
        public ActionResult<string> GetAuth()
        {
            return "test";
        }
        [HttpGet("not-found")]
        public ActionResult<UserDetails> GetAuthNotFound()
        {
            var th = _Context.UserDetails.Find(-1);
            if (th == null) return NotFound();
            return th;
        }
        [HttpGet("server-error")]
        public ActionResult<string> GetServererror()
        {
            var th = _Context.UserDetails.Find(-1) ?? throw new Exception("A Bad Request");
            return "test";
        }
        [HttpGet("Bad-Request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good request");
        }

    }
}
