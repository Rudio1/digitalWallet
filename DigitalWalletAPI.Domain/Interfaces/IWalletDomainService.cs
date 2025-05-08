using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Domain.Interfaces
{
    public interface IWalletDomainService
    {
        Task<Wallet> GetWalletByIdAsync(Guid walletId);
        Task<Wallet> GetWalletByUserIdAsync(Guid userId);
    }
} 