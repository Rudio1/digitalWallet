using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext _context;

        public TransactionRepository(TransactionContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Transactions
                .Where(t => t.SenderWalletId == walletId || t.ReceiverWalletId == walletId);

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedAt <= endDate.Value);

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
} 