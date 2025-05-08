using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWalletAPI.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
                return;

            var user1 = new User
            {
                Name = "João Silva",
                Email = "joao@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"),
                CreatedAt = DateTime.UtcNow
            };

            var user2 = new User
            {
                Name = "Maria Santos",
                Email = "maria@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Senha@123"),
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            var wallet1 = new Wallet
            {
                UserId = user1.Id,
                Balance = 1000.00m,
                CreatedAt = DateTime.UtcNow
            };

            var wallet2 = new Wallet
            {
                UserId = user2.Id,
                Balance = 500.00m,
                CreatedAt = DateTime.UtcNow
            };

            context.Wallets.AddRange(wallet1, wallet2);
            await context.SaveChangesAsync();

            var transaction1 = new Transaction
            {
                SenderWalletId = wallet1.Id,
                ReceiverWalletId = wallet2.Id,
                Amount = 100.00m,
                CreatedAt = DateTime.UtcNow
            };

            context.Transactions.Add(transaction1);
            await context.SaveChangesAsync();
        }
    }
} 