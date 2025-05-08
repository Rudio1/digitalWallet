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

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> GetByUserIdAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
                throw new KeyNotFoundException("Carteira não encontrada");

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
    }
} 