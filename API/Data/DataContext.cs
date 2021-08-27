using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) {

        }

        public DbSet<AppUser> Users {get; set;}

        public DbSet<UserFollow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<UserFollow>().HasKey(key => new {key.SourceUserId, key.FollowedUserId});

            builder.Entity<UserFollow>().HasOne(s => s.SourceUser).WithMany(f => f.UsersFollowed).HasForeignKey(s => s.SourceUserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollow>().HasOne(s => s.FollowedUser).WithMany(f => f.FollowedByUsers).HasForeignKey(s => s.FollowedUserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}