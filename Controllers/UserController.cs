using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace DatingApp.Controllers
{
    //[Authorize]
    public class UserController : BaseAPIController
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MerberDto>>> GetUsers()
        {
            var users = await _userRepository.GetUserAsync();
            if(users==null) return NotFound();
            List<MerberDto> merberDtos = new List<MerberDto>();
            foreach (UserDetails item in users)
            {
                MerberDto merberDto = new MerberDto
                {
                    Id = item.Id,
                    City = item.City,
                    Country = item.Country,
                    Gender = item.Gender,
                    Created = item.Created,
                    Interests = item.Interests,
                    Introduction = item.Introduction,
                    KnownAs = item.KnownAs,
                    LastActive = item.LastActive,
                    LookingFor = item.LookingFor,
                    PhotoUrl = null,
                    UserName = item.UserName,
                    Age = item.Age(),
                    Photos = []
                };
                if (item.Photos != null)
                {
                   
                    foreach (Photo photo in item.Photos)
                    {
                        PhotoDto ph = new PhotoDto
                        {
                            Id=photo.Id,
                            IsMain = photo.IsMain,
                            Url= photo.URL
                        
                        };
                        if(photo.IsMain==true) merberDto.PhotoUrl= photo.URL;
                        merberDto.Photos.Add(ph);

                    }
                }
                    merberDtos.Add(merberDto);
            }
           
    
            return Ok(merberDtos);



        }
        //[Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<MerberDto>> GetUesr(string username)
        {

            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return NotFound();
            MerberDto merberDto = new MerberDto
            {
                Age = user.Age(),
                City = user.City,
                Country = user.Country,
                Created = user.Created,
                Gender = user.Gender,
                Id = user.Id,
                Interests = user.Interests,
                Introduction = user.Introduction,
                KnownAs = user.KnownAs,
                LastActive = user.LastActive,
                LookingFor = user.LookingFor,
                PhotoUrl = null,
                UserName = user.UserName,
                Photos = []
            };
            foreach (Photo photo in user.Photos) {
                PhotoDto ph = new PhotoDto
                {
                    Id = photo.Id,
                    IsMain = photo.IsMain,
                    Url=photo.URL
                };
                if(photo.IsMain==true) merberDto.PhotoUrl= photo.URL;
                merberDto.Photos.Add(ph);
            }
            return merberDto;
        }
        [HttpPut("{username}")]

        public async Task<ActionResult> UpdateUser(string username, MemberUpdateDto memberUpdateDto)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (username == null) return BadRequest("No User found in token.");
            var user = await _userRepository.GetUserByUserNameAsync(username?.ToString());
            if (user == null) return BadRequest("Could not find User");
            _mapper.Map(memberUpdateDto, user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Faild to update User");

        }

    }
}
