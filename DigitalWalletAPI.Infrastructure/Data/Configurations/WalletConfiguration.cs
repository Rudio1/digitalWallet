using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Infrastructure.Data.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(w => w.Id);
            
            builder.Property(w => w.UserId)
                .IsRequired();

            builder.Property(w => w.Balance)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<Wallet>(w => w.UserId);
        }
    }
} 