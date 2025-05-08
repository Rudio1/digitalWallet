using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Infrastructure.Data;

namespace DigitalWalletAPI.Infrastructure.Repositories
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemConfigurationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SystemConfiguration> GetByKeyAsync(string key)
        {
            return await _context.SystemConfigurations
                .FirstOrDefaultAsync(x => x.Parameter == key);
        }

        public async Task<SystemConfiguration> CreateAsync(SystemConfiguration configuration)
        {
            await _context.SystemConfigurations.AddAsync(configuration);
            await _context.SaveChangesAsync();
            return configuration;
        }

        public async Task UpdateAsync(SystemConfiguration configuration)
        {
            _context.Entry(configuration).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
} 