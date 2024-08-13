using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Extension;
using DatingApp.Helpers;
using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace DatingApp.Controllers
{
    //[Authorize]
    public class UserController : BaseAPIController
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly IPhotoService _photoService;
        public UserController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }
        [AllowAnonymous]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MerberDto>>> GetUsers([FromQuery] UserParama userParama)
        //{
        //    var user = await _userRepository.GetMemberAsync(userParama);
        //    Response.AddPaginationHeader(user);
        //    return Ok(user);

        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MerberDto>>> GetUsers()
        {
            var users = await _userRepository.GetUserAsync();
            if (users == null) return NotFound();
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
                            Id = photo.Id,
                            IsMain = photo.IsMain,
                            Url = photo.URL

                        };
                        if (photo.IsMain == true) merberDto.PhotoUrl = photo.URL;
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
            foreach (Photo photo in user.Photos)
            {
                PhotoDto ph = new PhotoDto
                {
                    Id = photo.Id,
                    IsMain = photo.IsMain,
                    Url = photo.URL
                };
                if (photo.IsMain == true) merberDto.PhotoUrl = photo.URL;
                merberDto.Photos.Add(ph);
            }
            return merberDto;
        }
        [HttpPut("{username}")]

        public async Task<ActionResult> UpdateUser(string username, MemberUpdateDto memberUpdateDto)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (username == null) return BadRequest("No User found in token.");
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return BadRequest("Could not find User");
            _mapper.Map(memberUpdateDto, user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Faild to update User");

        }
       //http:5000/api/User/add-photo/username
        [HttpPost("add-photo/{username}")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(string username, IFormFile file)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return BadRequest("Could not find User");
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            Photo photo = new Photo
            {
                URL = result.SecureUrl.AbsoluteUri,
                PublicID = result.PublicId
            };
            if(user.Photos.Count==0) 
                photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync()) {
                PhotoDto photoDto = new PhotoDto
                {
                    Id = photo.Id,
                    IsMain = photo.IsMain,
                    Url = photo.URL,
                
                };
                return CreatedAtAction(nameof(GetUesr),new {username= user.UserName}, photoDto);
        }


            return BadRequest("Problem Add Your Photo");
        }

        [HttpPut("set-main-photo/{username}/{photoId:int}")]
        public async Task <ActionResult> SetPhoto(string username ,int photoId)
        {
            var user =await  _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return BadRequest("Could not find User");

            var Photo = user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if (Photo == null || Photo.IsMain) return BadRequest("Can not use this is main photo");
            var currentPhoto = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentPhoto!=null) currentPhoto.IsMain = false;
            Photo.IsMain = true;
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Problem are setting in main photo");
        }
        [HttpDelete("delete-photo/{username}/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(string username,int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null) return BadRequest("Could not find User");
            var Photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (Photo == null || Photo.IsMain) return BadRequest("Can not delete that photo");
            if(Photo.PublicID!=null)
            {
                var result = await _photoService.DeletePhotoAsync(Photo.PublicID);
                if(result.Error!=null) return  BadRequest(result.Error.Message);
            }
            user.Photos.Remove(Photo);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem for Delete Photo");
        }

    }

}
