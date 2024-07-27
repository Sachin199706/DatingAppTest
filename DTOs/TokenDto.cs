namespace DatingApp.DTOs
{
    public class TokenDto
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
        public string? photoUrl {  get; set; }
    }
}
