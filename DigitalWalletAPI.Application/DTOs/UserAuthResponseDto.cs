namespace DigitalWalletAPI.Application.DTOs
{
    public class UserAuthResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
    }
} 