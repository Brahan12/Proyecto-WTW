using System.Text.Json;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<int> CreateAsync(CreateTaskRequestDto dto)
        {
            var userExists = await _userRepository.ExistsAsync(dto.UserId);
            if (!userExists)
                throw new Exception("El usuario asignado no existe.");

            var entity = new TaskEntity
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                Status = TaskState.Pending,
                UserId = dto.UserId,
                ExtraData = dto.ExtraData
            };

            return await _taskRepository.CreateAsync(entity);
        }

        public async Task<List<TaskResponseDto>> GetAllAsync(string? status, string? priority)
        {
            var tasks = await _taskRepository.GetAllAsync(status, priority);

            return tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status.ToString(),
                UserId = t.UserId,
                ExtraData = t.ExtraData,
                Priority = ExtractPriority(t.ExtraData),
                CreateDate = t.CreateDate
            }).ToList();
        }

        private string? ExtractPriority(string? extraData)
        {
            if (string.IsNullOrWhiteSpace(extraData))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(extraData);

                if (doc.RootElement.TryGetProperty("priority", out var priority))
                    return priority.GetString();

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateStatusAsync(int id, string newStatus)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new Exception("La tarea no existe.");

            if (!Enum.TryParse<TaskState>(newStatus, out var parsedStatus))
                throw new Exception("Estado inválido.");

            if (task.Status == TaskState.Pending && parsedStatus == TaskState.Done)
                throw new Exception("No se permite cambiar una tarea directamente de Pending a Done.");

            return await _taskRepository.UpdateStatusAsync(id, parsedStatus);
        }
    }
}
