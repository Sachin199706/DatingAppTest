namespace DatingApp.Model
{
    public class UserLike
    {
        public UserDetails SourceUser { get; set; } = null!;
        public int SourceUserId { get; set; }
        public UserDetails TargetUser { get; set; } = null!;
        public int TargetUserId { get; set; }

    }
}
