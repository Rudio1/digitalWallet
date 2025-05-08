using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Infrastructure.Data.Contexts
{
    public class SystemConfigurationContext : DbContext
    {
        public SystemConfigurationContext(DbContextOptions<SystemConfigurationContext> options) : base(options)
        {
        }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemConfigurationContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
} 