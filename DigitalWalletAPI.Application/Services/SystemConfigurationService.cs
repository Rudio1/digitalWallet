using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.Interfaces;

namespace DigitalWalletAPI.Application.Services
{
    public class SystemConfigurationService : ISystemConfigurationService
    {
        private readonly ISystemConfigurationRepository _repository;

        public SystemConfigurationService(ISystemConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetValueAsync(string key)
        {
            var configuration = await _repository.GetByKeyAsync(key);
            return configuration.Value;
        }
    }
} 