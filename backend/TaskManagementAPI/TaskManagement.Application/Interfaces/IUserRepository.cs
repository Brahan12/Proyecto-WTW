using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(UserEntity user);
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
