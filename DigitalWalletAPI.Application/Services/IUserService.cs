using System;
using System.Threading.Tasks;
using DigitalWalletAPI.Application.DTOs;

namespace DigitalWalletAPI.Application.Services
{
    public interface IUserService
    {
        Task<UserAuthResponseDto> AuthenticateAsync(LoginUserDto loginDto);
        Task<UserDto> CreateAsync(CreateUserDto createDto);
        Task<UserDto> GetByIdAsync(Guid id);
    }
} 