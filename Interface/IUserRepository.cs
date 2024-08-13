using DatingApp.DTOs;
using DatingApp.Helpers;
using DatingApp.Model;
using System.Collections;

namespace DatingApp.Interface
{
    public interface IUserRepository
    {
        void Update(UserDetails user);
        Task<bool> SaveAllAsync();
        Task<UserDetails?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDetails>> GetUserAsync();
        Task<UserDetails> GetUserByUserNameAsync(string userName);
        Task<PageList<MerberDto>> GetMemberAsync(UserParama userParama);
        Task<MerberDto?> GetMemberAsync(string username);

    }
}
