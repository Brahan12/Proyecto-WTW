using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> CreateAsync(CreateUserRequestDto dto)
        {
            var entity = new UserEntity
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim()
            };

            return await _userRepository.CreateAsync(entity);
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                CreateDate = u.CreateDate
            }).ToList();
        }
    }
}
