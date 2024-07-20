using DatingApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApp.Data
{
    public class Seed
    {
        public static async Task ReadSeedData(DataContext _dataContext)
        {
            if(await _dataContext.UserDetails.AnyAsync()) return;
           var  userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            JsonSerializerOptions option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var user = JsonSerializer.Deserialize<List<UserDetails>>(userData, option);
            if(user==null) return;
            foreach (UserDetails item in user)
            {
                using var hmac = new HMACSHA512();
                item.UserName=item.UserName.ToLower();
                item.istrPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                item.istrPasswordSalt = hmac.Key;
                _dataContext.UserDetails.Add(item);
            }
            await _dataContext.SaveChangesAsync();  
        }
       
    }
}
