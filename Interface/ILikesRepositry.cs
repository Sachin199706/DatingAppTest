using DatingApp.DTOs;
using DatingApp.Model;

namespace DatingApp.Interface
{
    public interface ILikesRepositry
    {
        UserLike GetUserLike(int SourceId, int TargetId);
        Task<IEnumerable<MerberDto>> GetUserLikes(string predicate, int userID);
        Task<IEnumerable<int>> GetCurrentUserLikesId(int CurrentuserID);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
        Task<bool> SaveChanges();
    }
}
