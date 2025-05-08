using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Interfaces;
using DigitalWalletAPI.Domain.Entities;
using DigitalWalletAPI.Domain.Enums;
using DigitalWalletAPI.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

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
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                throw new UserException("Email ou senha inválidos", 401);

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
                throw new UserException("Email já está em uso", 400);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Email = createDto.Email,
                Password = HashPassword(createDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var initialBalanceValue = await _configService.GetValueAsync(SystemConstants.InitialBalanceParameter);
            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Balance = decimal.Parse(initialBalanceValue),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
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
                throw new UserException("Usuário não encontrado", 404);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            
            var issuer = await _configService.GetValueAsync(SystemConstants.JwtIssuerParameter);
            var audience = await _configService.GetValueAsync(SystemConstants.JwtAudienceParameter);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}