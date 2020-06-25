using Microsoft.EntityFrameworkCore;
using HobbyHub.Models;


namespace HobbyHub.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Hobbies> Hobbies { get; set; }
        public DbSet<HobbyHubAssoc> HobbyHubAssoc { get; set; }
    }
}