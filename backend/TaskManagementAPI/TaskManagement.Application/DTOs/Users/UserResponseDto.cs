
namespace TaskManagement.Application.DTOs.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset CreateDate { get; set; }
    }
}
