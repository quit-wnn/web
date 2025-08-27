using Microsoft.EntityFrameworkCore;
using Web.Models;
namespace Web.Models
{
   
        public class UserDbContext : DbContext
        {
            public UserDbContext(DbContextOptions<UserDbContext> options)
                : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<Tree> trees { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<CartItem> CartItems { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Tree)
                .WithMany()
                .HasForeignKey(c => c.PlantId);

            base.OnModelCreating(modelBuilder);
        }
    }
}