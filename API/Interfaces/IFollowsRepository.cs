using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IFollowsRepository
    {
        Task<UserFollow> GetUserFollow(int sourceUserId, int follwedUserId);

        Task<AppUser> GetUserWithFollows(int userId);

        Task<PagedList<FollowDTO>> GetUserFollows(FollowsParams followsParams);
    }
}