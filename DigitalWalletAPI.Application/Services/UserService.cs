using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Domain.Enums;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace DigitalWalletAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IConfiguration _configuration;
        private readonly ISystemConfigurationService _configService;

        public UserService(
            IUserRepository userRepository,
            IWalletRepository walletRepository,
            IConfiguration configuration,
            ISystemConfigurationService configService)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _configuration = configuration;
            _configService = configService;
        }

        public async Task<UserAuthResponseDto> AuthenticateAsync(LoginUserDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !BC.Verify(loginDto.Password, user.Password))
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            var token = await GenerateJwtToken(user);

            return new UserAuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createDto)
        {
            if (await _userRepository.ExistsAsync(createDto.Email))
                throw new InvalidOperationException("Email já está em uso");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Email = createDto.Email,
                Password = BC.HashPassword(createDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var initialBalanceValue = await _configService.GetValueAsync(SystemConstants.InitialBalanceParameter);
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Balance = decimal.Parse(initialBalanceValue),
                CreatedAt = DateTime.UtcNow
            };

            await _walletRepository.CreateAsync(wallet);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(await _configService.GetValueAsync(SystemConstants.JwtSecretKeyParameter));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(await _configService.GetValueAsync(SystemConstants.JwtExpirationInMinutesParameter))),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = await _configService.GetValueAsync(SystemConstants.JwtIssuerParameter),
                Audience = await _configService.GetValueAsync(SystemConstants.JwtAudienceParameter)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}