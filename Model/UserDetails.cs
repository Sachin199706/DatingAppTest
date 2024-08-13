using DatingApp.Extension;
using System.Text.Json.Serialization;

namespace DatingApp.Model
{
    public class UserDetails
    {
        public int Id { get; set; }

        public required string UserName { get; set; }
        public byte[] istrPasswordHash { get; set; } = [];
        public byte[] istrPasswordSalt { get; set; } = [];
        public DateOnly DateofBerth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime Created { get; set; }=DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public required string Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required  string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = [];
        public List<UserLike> LikedByUsers { get; set; } = [];
        public List<UserLike> LikedUsers { get; set; } = [];

        public  int Age()
        {
            return DateofBerth.CalculateAge();
        }

    }
}
