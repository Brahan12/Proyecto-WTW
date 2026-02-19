using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<int> CreateAsync(TaskEntity task);
        Task<List<TaskEntity>> GetAllAsync(string? status, string? priority);
        Task<TaskEntity?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, TaskState status);
    }
}
