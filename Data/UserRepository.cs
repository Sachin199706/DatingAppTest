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
    }
}
