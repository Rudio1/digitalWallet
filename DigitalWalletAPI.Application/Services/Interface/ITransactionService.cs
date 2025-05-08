using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.DTOs;

namespace DigitalWalletAPI.Application.Services
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateAsync(Guid senderWalletId, CreateTransactionDto createDto);
        Task<IEnumerable<TransactionDto>> GetByWalletIdAsync(Guid walletId, TransactionFilterDto filterDto);
        Task<TransactionDto> GetByIdAsync(Guid id);
    }
} 