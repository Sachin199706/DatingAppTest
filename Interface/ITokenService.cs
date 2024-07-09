using DatingApp.Model;

namespace DatingApp.Interface
{
    public interface ITokenService
    {
        string CreateToken(UserDetails user);
    }
}
