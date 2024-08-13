using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Interface;
using DatingApp.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class LikesRepository : ILikesRepositry
    {
        public readonly DataContext Context;
        public LikesRepository(DataContext dataContext)
        {
            Context = dataContext;
        }
        public void AddLike(UserLike like)
        {
            Context.Likes.Add(like);
        }

        public void DeleteLike(UserLike like)
        {
            if (like!=null)
            {
                Context.Likes.Remove(like);
            }
        }

      

        public async Task<IEnumerable<int>> GetCurrentUserLikesId(int CurrentuserID)
        {
            return await Context.Likes
                .Where(x => x.SourceUserId == CurrentuserID)
                .Select(x => x.TargetUserId).ToListAsync();
        }

        public UserLike GetUserLike(int SourceId, int TargetId)
        {
            return Context.Likes
            .SingleOrDefault(l => l.SourceUserId == SourceId && l.TargetUserId == TargetId);
        }

        public async Task<IEnumerable<MerberDto>> GetUserLikes(string predicate, int userID)
        {
            var likes = Context.Likes.AsQueryable();
            switch (predicate)
            {
                case "liked":
                    return await likes.Where(x => x.SourceUserId == userID)
                        .Select(x => new MerberDto
                        {
                            Id = x.TargetUser.Id,
                            Age = x.TargetUser.Age(),
                            City = x.TargetUser.City,
                            Country = x.TargetUser.Country,
                            Created = x.TargetUser.Created,
                            Gender = x.TargetUser.Gender,
                            Interests = x.TargetUser.Interests,
                            Introduction = x.TargetUser.Introduction,
                            KnownAs = x.TargetUser.KnownAs,
                            LastActive = x.TargetUser.LastActive,
                            LookingFor = x.TargetUser.LookingFor,
                            UserName = x.TargetUser.UserName,
                            Photos = x.TargetUser.Photos.Select(photo => new PhotoDto
                            {
                                Id = photo.Id,
                                Url = photo.URL,
                                IsMain = photo.IsMain
                                // Map other properties if necessary
                            }
                             ).ToList(),
                            PhotoUrl = x.TargetUser.Photos.FirstOrDefault(p => p.IsMain).URL,
                        }).ToListAsync();
                case "likedBy":
                    return await likes.Where(x => x.TargetUserId == userID)
                       .Select(x => new MerberDto
                       {
                           Id = x.SourceUser.Id,
                           Age = x.SourceUser.Age(),
                           City = x.SourceUser.City,
                           Country = x.SourceUser.Country,
                           Created = x.SourceUser.Created,
                           Gender = x.SourceUser.Gender,
                           Interests = x.SourceUser.Interests,
                           Introduction = x.SourceUser.Introduction,
                           KnownAs = x.SourceUser.KnownAs,
                           LastActive = x.SourceUser.LastActive,
                           LookingFor = x.SourceUser.LookingFor,
                           UserName = x.SourceUser.UserName,
                           Photos = x.SourceUser.Photos.Select(photo => new PhotoDto
                           {
                               Id = photo.Id,
                               Url = photo.URL,
                               IsMain = photo.IsMain
                               // Map other properties if necessary
                           }
                            ).ToList(),
                           PhotoUrl = x.SourceUser.Photos.FirstOrDefault(p => p.IsMain).URL,
                       }).ToListAsync();

                default:
                    var query = await GetCurrentUserLikesId(userID);
                    return await likes
                        .Where(x=>x.TargetUserId==userID && query.Contains(x.SourceUserId))
                        .Select(x => new MerberDto
                        {
                            Id = x.SourceUser.Id,
                            Age = x.SourceUser.Age(),
                            City = x.SourceUser.City,
                            Country = x.SourceUser.Country,
                            Created = x.SourceUser.Created,
                            Gender = x.SourceUser.Gender,
                            Interests = x.SourceUser.Interests,
                            Introduction = x.SourceUser.Introduction,
                            KnownAs = x.SourceUser.KnownAs,
                            LastActive = x.SourceUser.LastActive,
                            LookingFor = x.SourceUser.LookingFor,
                            UserName = x.SourceUser.UserName,
                            Photos = x.SourceUser.Photos.Select(photo => new PhotoDto
                            {
                                Id = photo.Id,
                                Url = photo.URL,
                                IsMain = photo.IsMain
                                // Map other properties if necessary
                            }
                            ).ToList(),
                            PhotoUrl = x.SourceUser.Photos.FirstOrDefault(p => p.IsMain).URL,
                        }).ToListAsync();
            }
        }

        public async Task<bool> SaveChanges()
        {
           return await Context.SaveChangesAsync()>0;
        }
    }
}
