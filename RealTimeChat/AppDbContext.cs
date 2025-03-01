using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;

namespace RealTimeChat
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Host=localhost;Port=5432;Database=chatapp_db;Username=your_username;Password=your_password";
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
