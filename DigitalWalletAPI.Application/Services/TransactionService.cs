using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Domain.Entities;

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
                throw new KeyNotFoundException("Carteira do remetente não encontrada");

            var receiverWallet = await _walletRepository.GetByIdAsync(createDto.ReceiverWalletId);
            if (receiverWallet == null)
                throw new KeyNotFoundException("Carteira do destinatário não encontrada");

            if (createDto.Amount <= 0)
                throw new InvalidOperationException("O valor da transferência deve ser maior que zero");

            if (senderWallet.Balance < createDto.Amount)
                throw new InvalidOperationException("Saldo insuficiente para realizar a transferência");

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SenderWalletId = senderWalletId,
                ReceiverWalletId = createDto.ReceiverWalletId,
                Amount = createDto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            senderWallet.Balance -= createDto.Amount;
            receiverWallet.Balance += createDto.Amount;

            await _walletRepository.UpdateAsync(senderWallet);
            await _walletRepository.UpdateAsync(receiverWallet);
            await _transactionRepository.CreateAsync(transaction);

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
            var transactions = await _transactionRepository.GetByWalletIdAsync(
                walletId,
                filterDto?.StartDate,
                filterDto?.EndDate);

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
                throw new KeyNotFoundException("Transação não encontrada");

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