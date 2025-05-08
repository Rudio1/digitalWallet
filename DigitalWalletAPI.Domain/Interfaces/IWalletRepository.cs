using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Domain.Interfaces;

public interface IWalletRepository
{
    Task<Wallet> GetByIdAsync(Guid id);
    Task<Wallet> GetByUserIdAsync(Guid userId);
    Task<decimal> GetBalanceAsync(Guid id);
    Task CreateAsync(Wallet wallet);
    Task UpdateAsync(Wallet wallet);
} 