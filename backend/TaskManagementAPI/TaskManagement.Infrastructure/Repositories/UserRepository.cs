using Dapper;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;


namespace TaskManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public UserRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(UserEntity user)
        {
            var sql = @"
                INSERT INTO dbo.Users (FullName, Email)
                VALUES (@FullName, @Email);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<List<UserEntity>> GetAllAsync()
        {
            var sql = @"
                SELECT Id, FullName, Email, CreateDate
                FROM dbo.Users
                ORDER BY CreateDate DESC;
            ";

            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<UserEntity>(sql);
            return result.ToList();
        }

        public async Task<UserEntity?> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT Id, FullName, Email, CreateDate
                FROM dbo.Users
                WHERE Id = @Id;
            ";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Id = id });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var sql = @"SELECT COUNT(1) FROM dbo.Users WHERE Id = @Id;";

            using var connection = _connectionFactory.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }
    }
}
