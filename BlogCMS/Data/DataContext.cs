
using BlogCMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Data
{
    public class DataContext : DbContext

    {
        public DataContext(DbContextOptions options):base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDto> UserDtos { get; set; }
    }
}
