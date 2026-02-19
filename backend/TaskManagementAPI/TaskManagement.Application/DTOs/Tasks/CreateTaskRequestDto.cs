
namespace TaskManagement.Application.DTOs.Tasks
{
    public class CreateTaskRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public string? ExtraData { get; set; }
    }
}
