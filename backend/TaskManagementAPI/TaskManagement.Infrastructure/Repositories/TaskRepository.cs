using Dapper;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public TaskRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(TaskEntity task)
        {
            var sql = @"
                INSERT INTO dbo.Tasks (Title, Description, Status, UserId, ExtraData)
                VALUES (@Title, @Description, @Status, @UserId, @ExtraData);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                task.Title,
                task.Description,
                Status = task.Status.ToString(),
                task.UserId,
                task.ExtraData
            });
        }

        public async Task<List<TaskEntity>> GetAllAsync(string? status = null, string? priority = null)
        {
            var sql = @"
                SELECT Id, Title, Description, Status, UserId, ExtraData, CreateDate
                FROM dbo.Tasks
                WHERE 1 = 1
            ";

            if (!string.IsNullOrWhiteSpace(status))
                sql += " AND Status = @Status ";

            if (!string.IsNullOrWhiteSpace(priority))
                sql += " AND JSON_VALUE(ExtraData, '$.priority') = @Priority ";

            sql += " ORDER BY CreateDate DESC; ";

            using var connection = _connectionFactory.CreateConnection();

            var dbResult = await connection.QueryAsync<TaskDbModel>(sql, new
            {
                Status = status,
                Priority = priority
            });

            var tasks = dbResult.Select(x => new TaskEntity
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Status = Enum.Parse<TaskState>(x.Status),
                UserId = x.UserId,
                ExtraData = x.ExtraData,
                CreateDate = x.CreateDate
            }).ToList();

            return tasks;
        }

        public async Task<TaskEntity?> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT Id, Title, Description, Status, UserId, ExtraData, CreateDate
                FROM dbo.Tasks
                WHERE Id = @Id;
            ";

            using var connection = _connectionFactory.CreateConnection();

            var taskDb = await connection.QueryFirstOrDefaultAsync<TaskDbModel>(sql, new { Id = id });

            if (taskDb == null)
                return null;

            return new TaskEntity
            {
                Id = taskDb.Id,
                Title = taskDb.Title,
                Description = taskDb.Description,
                Status = Enum.Parse<TaskState>(taskDb.Status),
                UserId = taskDb.UserId,
                ExtraData = taskDb.ExtraData,
                CreateDate = taskDb.CreateDate
            };
        }

        public async Task<bool> UpdateStatusAsync(int id, TaskState status)
        {
            var sql = @"
                UPDATE dbo.Tasks
                SET Status = @Status
                WHERE Id = @Id;
            ";

            using var connection = _connectionFactory.CreateConnection();

            var rows = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                Status = status.ToString()
            });

            return rows > 0;
        }
    }
}
