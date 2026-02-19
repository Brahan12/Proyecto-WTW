
namespace TaskManagement.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset CreateDate { get; set; }
    }
}
