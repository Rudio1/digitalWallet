using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Domain.Interfaces;
using DigitalWalletAPI.Domain.Exceptions;
using DigitalWalletAPI.Domain.Services;

namespace DigitalWalletAPI.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(
            IWalletRepository walletRepository
            )
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> GetByUserIdAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            return new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
        }

        public async Task<decimal> GetBalanceAsync(Guid walletId)
        {
            var wallet = await VerifyIfWalletExists(walletId);
            return await _walletRepository.GetBalanceAsync(walletId);
        }

        public async Task<WalletDto> AddBalanceAsync(Guid walletId, AddBalanceDto addBalanceDto)
        {
            var wallet = await VerifyIfWalletExists(walletId);
            wallet.AddBalance(addBalanceDto.Amount);
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
            var wallet = await VerifyIfWalletExists(walletId);
            wallet.UpdateBalance(newBalance);
            await _walletRepository.UpdateAsync(wallet);
        }

        private async Task<Wallet> VerifyIfWalletExists(Guid walletId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
                throw new WalletNotFoundException();

            return wallet;
        }
    }
} 