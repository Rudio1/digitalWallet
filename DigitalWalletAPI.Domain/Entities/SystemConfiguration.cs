using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalWalletAPI.Domain.Entities
{
    public class SystemConfiguration
    {
        public Guid Id { get; set; }
        [Required]
        public string Parameter { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 