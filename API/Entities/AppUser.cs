using System;
using System.Collections.Generic;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Photo Photo { get; set; }
        public ICollection<UserFollow> FollowedByUsers { get; set; }
        public ICollection<UserFollow> UsersFollowed { get; set; }
    }   
}