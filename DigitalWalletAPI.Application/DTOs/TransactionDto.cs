using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalWalletAPI.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid SenderWalletId { get; set; }
        public Guid ReceiverWalletId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "A carteira de destino é obrigatória")]
        public Guid ReceiverWalletId { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Amount { get; set; }
    }

    public class TransactionFilterDto
    {
        [DataType(DataType.Date, ErrorMessage = "Data inicial inválida")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Data final inválida")]
        public DateTime? EndDate { get; set; }
    }
} 