using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Transaction> GetByIdAsync(Guid id);
    }
} 