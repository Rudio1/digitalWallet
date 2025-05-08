using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Infrastructure.Data.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
} 