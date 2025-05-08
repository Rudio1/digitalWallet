using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Domain.Interfaces;
using DigitalWalletAPI.Domain.Exceptions;

namespace DigitalWalletAPI.Domain.Services
{
    public class WalletDomainService : IWalletDomainService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletDomainService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Wallet> GetWalletByIdAsync(Guid walletId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
                throw new WalletNotFoundException();

            return wallet;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
                throw new WalletNotFoundException();

            return wallet;
        }
    }
} 