using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDTO, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to update");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());    
            await _photoService.DeletePhotoAsync(user.Photo.PublicId);  
            var result = await _photoService.ChangePhotoAsync(file);
            if(result.Error != null) {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            
            user.Photo.Url = photo.Url; 
            user.Photo.PublicId = photo.PublicId;

            
            if(await _userRepository.SaveAllAsync()) {
                return CreatedAtRoute("GetUser",new {userName = user.UserName} ,_mapper.Map<PhotoDTO>(photo));
            }

            return BadRequest("Could not upload photo");
        }
    }
}