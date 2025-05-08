using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _context;

        public WalletRepository(WalletContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetByIdAsync(Guid id)
        {
            return await _context.Wallets.FindAsync(id);
        }

        public async Task<Wallet> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<decimal> GetBalanceAsync(Guid walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            return wallet?.Balance ?? 0;
        }

        public async Task<Wallet> CreateAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
} 