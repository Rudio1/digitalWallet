using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Domain.Interfaces;
using DigitalWalletAPI.Domain.Exceptions;

namespace DigitalWalletAPI.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
        }

        public async Task<TransactionDto> CreateAsync(Guid senderWalletId, CreateTransactionDto createDto)
        {
            var senderWallet = await _walletRepository.GetByIdAsync(senderWalletId);
            if (senderWallet == null)
                throw new WalletNotFoundException();

            var receiverWallet = await _walletRepository.GetByIdAsync(createDto.ReceiverWalletId);
            if (receiverWallet == null)
                throw new WalletNotFoundException();

            if (senderWallet.Balance < createDto.Amount)
                throw new InsufficientBalanceException(senderWalletId, "Saldo insuficiente para realizar a transação");

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SenderWalletId = senderWalletId,
                ReceiverWalletId = createDto.ReceiverWalletId,
                Amount = createDto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.CreateAsync(transaction);

            senderWallet.Debit(createDto.Amount);
            receiverWallet.Credit(createDto.Amount);

            await _walletRepository.UpdateAsync(senderWallet);
            await _walletRepository.UpdateAsync(receiverWallet);

            return new TransactionDto
            {
                Id = transaction.Id,
                SenderWalletId = transaction.SenderWalletId,
                ReceiverWalletId = transaction.ReceiverWalletId,
                Amount = transaction.Amount,
                CreatedAt = transaction.CreatedAt
            };
        }

        public async Task<IEnumerable<TransactionDto>> GetByWalletIdAsync(Guid walletId, TransactionFilterDto filterDto)
        {
            var transactions = await _transactionRepository.GetByWalletIdAsync(walletId, filterDto?.StartDate, filterDto?.EndDate);
            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                SenderWalletId = t.SenderWalletId,
                ReceiverWalletId = t.ReceiverWalletId,
                Amount = t.Amount,
                CreatedAt = t.CreatedAt
            });
        }

        public async Task<TransactionDto> GetByIdAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                throw new TransactionNotFoundException(id, "Transação não encontrada");

            return new TransactionDto
            {
                Id = transaction.Id,
                SenderWalletId = transaction.SenderWalletId,
                ReceiverWalletId = transaction.ReceiverWalletId,
                Amount = transaction.Amount,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
} 