using DatingApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace DatingApp.Data
{
    
        public class DataContext : DbContext
        {
        public DataContext(DbContextOptions option) : base(option)
        {

        }
            public DbSet<AppUser>  users { get; set; }
            public DbSet<AppUsers> User { get; set; }
    
        }
    
}
