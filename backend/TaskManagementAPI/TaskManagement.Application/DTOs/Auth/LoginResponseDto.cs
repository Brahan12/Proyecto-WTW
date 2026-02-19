
namespace TaskManagement.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset Expiration { get; set; }
    }
}
