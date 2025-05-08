using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Infrastructure.Data.Contexts
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
} 