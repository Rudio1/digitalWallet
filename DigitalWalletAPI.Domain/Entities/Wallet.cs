using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Exceptions;
using DigitalWalletAPI.Domain.Interfaces;

namespace DigitalWalletAPI.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Wallet() { } // Para o EF

        public Wallet(Guid userId, decimal initialBalance)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Balance = initialBalance;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddBalance(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidWalletOperationException("O valor deve ser maior que zero");

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateBalance(decimal newBalance)
        {
            if (newBalance < 0)
                throw new WalletException("O saldo não pode ser negativo");

            Balance = newBalance;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Debit(decimal amount)
        {

            if (Balance < amount)
                throw new InsufficientBalanceException(Id, "Saldo insuficiente para realizar a operação");

            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidWalletOperationException("O valor recebido deve ser maior que zero");

            AddBalance(amount);
        }
    }
} 