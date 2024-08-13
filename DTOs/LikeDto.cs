using DatingApp.Model;

namespace DatingApp.DTOs
{
    public class LikeDto
    {
        public UserDetails SourceUser { get; set; } = null!;
        public int SourceUserId { get; set; }
        public UserDetails TargetUser { get; set; } = null!;
        public int TargetUserId { get; set; }
    }
}
