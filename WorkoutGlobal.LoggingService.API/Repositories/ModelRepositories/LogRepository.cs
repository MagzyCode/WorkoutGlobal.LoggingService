﻿using Dapper;
using System.Data;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Repositories
{
    /// <summary>
    /// Represents repository for log model.
    /// </summary>
    public class LogRepository : BaseRepository, ILogRepository<Guid, Log>
    {
        /// <summary>
        /// Ctor for log repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="tableName">Table name.</param>
        public LogRepository(IConfiguration configuration, string tableName = "Logs") 
            : base(configuration, tableName)
        { }

        /// <summary>
        /// Create log.
        /// </summary>
        /// <param name="creationLog">Creation log.</param>
        /// <returns>Return generated id for new model.</returns>
        public async Task<Guid> CreateLogAsync(Log creationLog)
        {
            if (creationLog is null)
                throw new ArgumentNullException(nameof(creationLog), "Creating log model cannot be null.");

            var query = @$"INSERT INTO {TableName} (""Id"", ""LogTime"", ""Message"", ""SeverityId"") 
                           VALUES (@Id, @LogTime, @Message, @SeverityId)
                           RETURNING ""Id""";

            var parameters = new DynamicParameters();
            parameters.Add("Id", Guid.NewGuid(), DbType.Guid);
            parameters.Add("LogTime", creationLog.LogTime, DbType.DateTime);
            parameters.Add("Message", creationLog.Message, DbType.String);
            parameters.Add("SeverityId", creationLog.SeverityId, DbType.Int32);

            using var connection = OpenConnection();

            var createdId = await connection.ExecuteScalarAsync<Guid>(query, parameters);

            return createdId;
        }

        /// <summary>
        /// Delete log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns></returns>
        public async Task DeleteLogAsync(Guid id)
        {
            if (Guid.Empty == id)
                throw new ArgumentNullException(nameof(id), "Delete log id cannot be empty.");

            var query = @$"DELETE FROM {TableName} WHERE ""Id"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            using var connection = OpenConnection();

            var affectedRows = await connection.ExecuteAsync(query, parameters);

            if (affectedRows != 1)
                throw new InvalidOperationException("Delete operation don't execute.");
        }

        /// <summary>
        /// Get all logs.
        /// </summary>
        /// <returns>Returns collection of all logs.</returns>
        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            var query = @$"SELECT * FROM {TableName}";

            using var connection = OpenConnection();
            var logs = await connection.QueryAsync<Log>(query);

            return logs.ToList();
        }

        /// <summary>
        /// Get log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Returns log by given id.</returns>
        public async Task<Log> GetLogAsync(Guid id)
        {
            if (Guid.Empty == id)
                throw new ArgumentNullException(nameof(id), "Log id cannot be empty.");

            var query = @$"SELECT * FROM {TableName} WHERE ""Id"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            using var connection = OpenConnection();
            var log = await connection.QuerySingleOrDefaultAsync<Log>(query, parameters);

            return log;
        }

        /// <summary>
        /// Get severity level of log.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Severity level of log.</returns>
        public async Task<Severity> GetLogSevetiry(Guid id)
        {
            if (Guid.Empty == id)
                throw new ArgumentNullException(nameof(id), "Log id cannot be empty.");

            var log = await GetLogAsync(id);

            var query = @"SELECT * FROM ""Severities"" WHERE ""Id"" = @SeverityId";

            var parameters = new DynamicParameters();
            parameters.Add("Id", log.SeverityId, DbType.Int32);

            using var connection = OpenConnection();
            var severity = await connection.QuerySingleOrDefaultAsync<Severity>(query, parameters);

            return severity;
        }

        /// <summary>
        /// Update log.
        /// </summary>
        /// <param name="updationLog">Updation log.</param>
        /// <returns></returns>
        public async Task UpdateLogAsync(Log updationLog)
        {
            if (updationLog is null)
                throw new ArgumentNullException(nameof(updationLog), "Updation log model cannot be null.");

            var query = @$"UPDATE {TableName} 
                           SET ""Id"" = @Id, ""LogTime"" = @LogTime, ""Message"" = @Message, ""SeverityId"" = @SeverityId
                           WHERE ""Id"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", updationLog.Id, DbType.Guid);
            parameters.Add("LogTime", updationLog.LogTime, DbType.DateTime);
            parameters.Add("Message", updationLog.Message, DbType.String);
            parameters.Add("SeverityId", updationLog.SeverityId, DbType.Int32);

            using var connection = OpenConnection();

            var affectedRows = await connection.ExecuteAsync(query, parameters);

            if (affectedRows != 1)
                throw new InvalidOperationException("Update operation don't execute.");
        }
    }
}
