using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskState Status { get; set; }
        public int UserId { get; set; }
        public string? ExtraData { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
