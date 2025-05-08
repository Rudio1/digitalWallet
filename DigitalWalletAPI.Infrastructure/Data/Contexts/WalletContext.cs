using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Infrastructure.Data.Contexts
{
    public class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
} 