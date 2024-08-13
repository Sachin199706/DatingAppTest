using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DatingApp.Data
{
    public class UserRepository : IUserRepository
    {
        public DataContext Context { get; set; }

        public UserRepository(DataContext dataContext) 
        {
            Context = dataContext;
        }
        public async Task<IEnumerable<UserDetails>> GetUserAsync()
        {
            return await Context.UserDetails.
                Include(x=>x.Photos).
                ToListAsync();  
        }

        public async Task<UserDetails?> GetUserByIdAsync(int id)
        {
            return await Context.UserDetails.FindAsync(id);
        }

        public async Task<UserDetails?> GetUserByUserNameAsync(string username)
        {
            return await Context.UserDetails.
                Include(x=>x.Photos).SingleOrDefaultAsync(x=>x.UserName== username);
        }

        public async Task<bool>  SaveAllAsync()
        {
          return await Context.SaveChangesAsync() > 0;
        }

       public async void Update(UserDetails user)
        {
           Context.Entry(user).State=EntityState.Modified;
        }

        public async Task<PageList<MerberDto>> GetMemberAsync(UserParama userParama)
        {

            var query = Context.UserDetails.Select(user => new MerberDto
            {
                Id= user.Id,
                City=user.City,
                Country=user.Country,
                Interests=user.Interests,
                Created = user.Created,
                Gender = user.Gender,
                Introduction = user.Introduction,
                KnownAs=user.KnownAs,
                LastActive=user.LastActive,
                UserName=user.UserName,
                Age=user.Age(),
                LookingFor=user.LookingFor,
                Photos = user.Photos.Select(photo => new PhotoDto
                {
                    Id = photo.Id,
                    Url = photo.URL,
                    IsMain = photo.IsMain
                    // Map other properties if necessary
                }
                ).ToList(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).URL,
            });
                return await PageList<MerberDto>.CreateAsync(query,userParama.PageNumber,userParama.PageSize);
        }

        public async Task<MerberDto?> GetMemberAsync(string username)
        {
            var user = await Context.UserDetails
        .Include(u => u.Photos) // Include Photos if they are lazy-loaded
        .FirstOrDefaultAsync(u => u.UserName == username);

            // If the user is not found, return null
            if (user == null) return null;

            // Map UserDetails to MemberDto
            var memberDto = new MerberDto
            {
                UserName = user.UserName,
                Id = user.Id,
                City = user.City,
                Country = user.Country,
                Age = user.Age(), // Assuming Age() is a method in UserDetails
                Created = user.Created,
                Gender = user.Gender,
                Interests = user.Interests,
                Introduction = user.Introduction,
                KnownAs = user.KnownAs,
                LastActive = user.LastActive,
                LookingFor = user.LookingFor,
                PhotoUrl = user.Photos?.FirstOrDefault(p => p.IsMain)?.URL, // Assuming you want the main photo URL
                Photos = user.Photos.Select(photo => new PhotoDto
                {
                    Id = photo.Id,
                    IsMain = photo.IsMain,
                    Url = photo.URL
                }).ToList()
            };

            return memberDto;
        }
    }
}
