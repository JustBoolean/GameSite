using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FollowsRepository : IFollowsRepository
    {
        private readonly DataContext _context;
        public FollowsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserFollow> GetUserFollow(int sourceUserId, int follwedUserId)
        {
            return await _context.Follows.FindAsync(sourceUserId, follwedUserId);
        }

        public async Task<PagedList<FollowDTO>> GetUserFollows(FollowsParams followsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var follows = _context.Follows.AsQueryable();

            if(followsParams.Predicate == "followed") {
                follows = follows.Where(follow => follow.SourceUserId == followsParams.UserId);
                users = follows.Select( follow => follow.FollowedUser);
            }

            if(followsParams.Predicate == "followedBy") {
                follows = follows.Where(follow => follow.FollowedUserId == followsParams.UserId);
                users = follows.Select( follow => follow.SourceUser);
            }

            var followedUsers = users.Select(user => new FollowDTO {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photo.Url,
                City = user.City,
                Id = user.Id         
            });

            return await PagedList<FollowDTO>.CreateAsync(followedUsers, followsParams.PageNumber, followsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithFollows(int userId)
        {
            return await _context.Users.Include(x => x.UsersFollowed).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}