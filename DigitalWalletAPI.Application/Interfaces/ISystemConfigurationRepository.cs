using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Application.Interfaces
{
    public interface ISystemConfigurationRepository
    {
        Task<SystemConfiguration> GetByKeyAsync(string key);
    }
} 