using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
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

        public async Task<IEnumerable<FollowDTO>> GetUserFollows(string predicate, int userId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var follows = _context.Follows.AsQueryable();

            if(predicate == "followed") {
                follows = follows.Where(follow => follow.SourceUserId == userId);
                users = follows.Select( follow => follow.FollowedUser);
            }

            if(predicate == "followedBy") {
                follows = follows.Where(follow => follow.FollowedUserId == userId);
                users = follows.Select( follow => follow.SourceUser);
            }

            return await users.Select(user => new FollowDTO {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photo.Url,
                City = user.City,
                Id = user.Id         
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithFollows(int userId)
        {
            return await _context.Users.Include(x => x.UsersFollowed).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}