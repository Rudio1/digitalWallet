using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Application.DTOs;

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
                throw new Exception("Carteira do remetente não encontrada");

            var receiverWallet = await _walletRepository.GetByIdAsync(createDto.ReceiverWalletId);
            if (receiverWallet == null)
                throw new Exception("Carteira do destinatário não encontrada");

            if (senderWallet.Balance < createDto.Amount)
                throw new Exception("Saldo insuficiente");

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SenderWalletId = senderWalletId,
                ReceiverWalletId = createDto.ReceiverWalletId,
                Amount = createDto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.CreateAsync(transaction);

            // Atualiza os saldos das carteiras
            senderWallet.Balance -= createDto.Amount;
            receiverWallet.Balance += createDto.Amount;
            senderWallet.UpdatedAt = DateTime.UtcNow;
            receiverWallet.UpdatedAt = DateTime.UtcNow;

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