using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class FollowsController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowsRepository _followsRepository;
        public FollowsController(IUserRepository userRepository, IFollowsRepository followsRepository)
        {
            _followsRepository = followsRepository;
            _userRepository = userRepository;
        }
        
        [HttpPost("{username}")]
        public async Task<ActionResult> AddFollow(string username) {
            var sourceUserId = User.GetUserId();
            var followedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _followsRepository.GetUserWithFollows(sourceUserId);

            if(followedUser == null) {
                return NotFound();
            }

            if(sourceUser.UserName == username){
                return BadRequest("Cannot follow yourself");
            }

            var userFollow = await _followsRepository.GetUserFollow(sourceUserId, followedUser.Id);

            if(userFollow != null) {
                return BadRequest("You already followed this user");
            }

            userFollow = new UserFollow {
                SourceUserId = sourceUserId,
                FollowedUserId = followedUser.Id
            };

            sourceUser.UsersFollowed.Add(userFollow);

            if (await _userRepository.SaveAllAsync()) {
                return Ok();
            }

            return BadRequest("Failed to follow user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowDTO>>> GetUserFollows([FromQuery]FollowsParams followsParams)  {
            followsParams.UserId = User.GetUserId();
            var users =  await _followsRepository.GetUserFollows(followsParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

    }
}