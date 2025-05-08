using System.Threading.Tasks;

namespace DigitalWalletAPI.Application.Interfaces
{
    public interface ISystemConfigurationService
    {
        Task<string> GetValueAsync(string key);
    }
} 