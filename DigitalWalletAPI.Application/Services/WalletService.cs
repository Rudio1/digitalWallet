using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Domain.Entities;

namespace DigitalWalletAPI.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISystemConfigurationService _configService;

        public WalletService(
            IWalletRepository walletRepository,
            IUserRepository userRepository,
            ISystemConfigurationService configService)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _configService = configService;
        }

        public async Task<WalletDto> CreateWalletAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            var existingWallet = await _walletRepository.GetByUserIdAsync(userId);
            if (existingWallet != null)
                throw new Exception("Usuário já possui uma carteira");

            var initialBalance = decimal.Parse(await _configService.GetValueAsync("initial_wallet_balance"));

            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = initialBalance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _walletRepository.CreateAsync(wallet);

            return new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
        }

        public async Task<WalletDto> GetByUserIdAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
                throw new Exception("Carteira não encontrada");

            return new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
        }

        public async Task<decimal> GetBalanceAsync(Guid walletId)
        {
            return await _walletRepository.GetBalanceAsync(walletId);
        }

        public async Task<WalletDto> AddBalanceAsync(Guid walletId, AddBalanceDto addBalanceDto)
        {
            if (addBalanceDto.Amount <= 0)
                throw new InvalidOperationException("O valor deve ser maior que zero");

            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
                throw new KeyNotFoundException("Carteira não encontrada");

            wallet.Balance += addBalanceDto.Amount;
            wallet.UpdatedAt = DateTime.UtcNow;

            await _walletRepository.UpdateAsync(wallet);

            return new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
        }

        public async Task UpdateBalanceAsync(Guid walletId, decimal newBalance)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
                throw new Exception("Carteira não encontrada");

            wallet.Balance = newBalance;
            wallet.UpdatedAt = DateTime.UtcNow;
            await _walletRepository.UpdateAsync(wallet);
        }
    }
} 