
namespace TaskManagement.Infrastructure.Data
{
    public class TaskDbModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? ExtraData { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
