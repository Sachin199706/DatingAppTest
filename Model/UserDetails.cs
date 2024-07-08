namespace DatingApp.Model
{
    public class UserDetails
    {
        public int Id { get; set; }

        public required string UserName { get; set; }
        public required byte[] istrPasswordHash { get; set; }
        public required byte[] istrPasswordSalt { get; set; }
    }
}
