using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Infrastructure.Repositories
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly SystemConfigurationContext _context;

        public SystemConfigurationRepository(SystemConfigurationContext context)
        {
            _context = context;
        }

        public async Task<SystemConfiguration> GetByKeyAsync(string key)
        {
            return await _context.SystemConfigurations
                .FirstOrDefaultAsync(c => c.Parameter == key);
        }

        public async Task<SystemConfiguration> CreateAsync(SystemConfiguration configuration)
        {
            await _context.SystemConfigurations.AddAsync(configuration);
            await _context.SaveChangesAsync();
            return configuration;
        }

        public async Task UpdateAsync(SystemConfiguration configuration)
        {
            _context.SystemConfigurations.Update(configuration);
            await _context.SaveChangesAsync();
        }
    }
} 