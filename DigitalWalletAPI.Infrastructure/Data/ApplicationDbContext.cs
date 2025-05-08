using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.User)
                    .WithOne(e => e.Wallet)
                    .HasForeignKey<Wallet>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.SenderWallet)
                    .WithMany(e => e.SentTransactions)
                    .HasForeignKey(e => e.SenderWalletId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ReceiverWallet)
                    .WithMany(e => e.ReceivedTransactions)
                    .HasForeignKey(e => e.ReceiverWalletId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SystemConfiguration>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Parameter).IsRequired();
                entity.Property(e => e.Value).IsRequired();
                entity.HasIndex(e => e.Parameter).IsUnique();
            });
        }
    }
} 