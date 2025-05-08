using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> ExistsAsync(string email);
    }
} 