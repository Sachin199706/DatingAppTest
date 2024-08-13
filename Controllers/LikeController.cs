using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    public class LikeController : BaseAPIController
    {
     
        public readonly ILikesRepositry likesRepositry;
        public readonly IUserRepository _userRepository;
        public LikeController(ILikesRepositry likes, IUserRepository userRepository)
        {
            likesRepositry = likes;
            _userRepository = userRepository;
        }
        [HttpPost("likes/{username}/{TargetUserId:int}")]
        public async Task<ActionResult> ToggleLike(string username, int TargetUserId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user.Id == TargetUserId) return BadRequest("You Can not Like your self");
            var exitLikes = likesRepositry.GetUserLike(user.Id, TargetUserId);
            if (exitLikes == null)
            {
                var like = new UserLike
                {
                    SourceUserId = user.Id,
                    TargetUserId = TargetUserId,
                };
                likesRepositry.AddLike(like);
            }
            else
            {

                likesRepositry.DeleteLike(exitLikes);
            }
            if (await likesRepositry.SaveChanges()) return Ok();
            return BadRequest("Failed to update Like");
        }
        [HttpGet("list/{username}")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeId(string username)
        {
            var user = _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return BadRequest("No Data Found");
            return Ok(await likesRepositry.GetCurrentUserLikesId(user.Id));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MerberDto>>> GetUserName(string username,string predicate)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return BadRequest("No Data Found");
            var users = await likesRepositry.GetUserLikes(predicate, user.Id);
            return Ok(users);
        }
    }
}
