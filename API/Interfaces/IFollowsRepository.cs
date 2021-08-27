using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;

namespace API.Interfaces
{
    public interface IFollowsRepository
    {
        Task<UserFollow> GetUserFollow(int sourceUserId, int follwedUserId);

        Task<AppUser> GetUserWithFollows(int userId);

        Task<IEnumerable<FollowDTO>> GetUserFollows(string predicate, int userId);
    }
}