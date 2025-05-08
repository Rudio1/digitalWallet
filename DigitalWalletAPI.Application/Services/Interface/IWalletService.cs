using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.DTOs;

namespace DigitalWalletAPI.Application.Services
{
    public interface IWalletService
    {
        Task<WalletDto> GetByUserIdAsync(Guid userId);
        Task<decimal> GetBalanceAsync(Guid walletId);
        Task<WalletDto> AddBalanceAsync(Guid walletId, AddBalanceDto addBalanceDto);
        Task UpdateBalanceAsync(Guid walletId, decimal newBalance);
    }
} 