using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWalletAPI.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var userContext = services.GetRequiredService<UserContext>();
            var walletContext = services.GetRequiredService<WalletContext>();
            var transactionContext = services.GetRequiredService<TransactionContext>();

            if (await userContext.Users.AnyAsync())
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

            userContext.Users.AddRange(user1, user2);
            await userContext.SaveChangesAsync();

            var wallet1 = new Wallet(user1.Id, 1000.00m);
            var wallet2 = new Wallet(user2.Id, 500.00m);

            walletContext.Wallets.AddRange(wallet1, wallet2);
            await walletContext.SaveChangesAsync();

            var transaction1 = new Transaction
            {
                SenderWalletId = wallet1.Id,
                ReceiverWalletId = wallet2.Id,
                Amount = 100.00m,
                CreatedAt = DateTime.UtcNow
            };

            transactionContext.Transactions.Add(transaction1);
            await transactionContext.SaveChangesAsync();
        }
    }
} 