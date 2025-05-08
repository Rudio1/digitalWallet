using System;

namespace DigitalWalletAPI.Application.DTOs
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }

    public class AddBalanceDto
    {
        public decimal Amount { get; set; }
    }
} 