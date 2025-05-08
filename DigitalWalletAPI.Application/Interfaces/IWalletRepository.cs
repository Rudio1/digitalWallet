using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByIdAsync(Guid id);
        Task<Wallet> GetByUserIdAsync(Guid userId);
        Task<Wallet> CreateAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
        Task<decimal> GetBalanceAsync(Guid walletId);
    }
} 