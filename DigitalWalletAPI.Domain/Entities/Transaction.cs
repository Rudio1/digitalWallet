using System;

namespace DigitalWalletAPI.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid SenderWalletId { get; set; }
        public Guid ReceiverWalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 