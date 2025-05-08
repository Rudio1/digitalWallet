using System;

namespace DigitalWalletAPI.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid SenderWalletId { get; set; }
        public Guid ReceiverWalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTransactionDto
    {
        public Guid ReceiverWalletId { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactionFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
} 