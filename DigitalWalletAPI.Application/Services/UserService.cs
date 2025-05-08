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
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using DigitalWalletAPI.Domain.Interfaces;

namespace DigitalWalletAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IConfiguration _configuration;
        private readonly ISystemConfigurationService _configService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IWalletRepository walletRepository,
            IConfiguration configuration,
            ISystemConfigurationService configService,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _configuration = configuration;
            _configService = configService;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserAuthResponseDto> AuthenticateAsync(LoginUserDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(user, loginDto.Password))
                throw new UserException("Email ou senha inválidos", 401);

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            return new UserAuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.Password = HashPassword(user, createDto.Password);
            await _userRepository.CreateAsync(user);

            var initialBalanceValue = await _configService.GetValueAsync(SystemConstants.InitialBalanceParameter);
            if (!decimal.TryParse(initialBalanceValue, out decimal initialBalance))
            {
                initialBalance = 0;
            }

            var wallet = new Wallet(user.Id, initialBalance);
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

        private string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        private bool VerifyPassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result switch
            {
                PasswordVerificationResult.Success => true,
                PasswordVerificationResult.Failed => false,
                PasswordVerificationResult.SuccessRehashNeeded => true,
                _ => false
            };
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

        private string GenerateRefreshToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[32];
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
