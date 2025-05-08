using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalWalletAPI.Application.DTOs
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Balance { get; set; }
    }

    public class AddBalanceDto
    {
        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Amount { get; set; }
    }
} 