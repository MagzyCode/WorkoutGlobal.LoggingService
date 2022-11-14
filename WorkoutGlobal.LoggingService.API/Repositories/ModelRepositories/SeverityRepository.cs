using Dapper;
using System.Data;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Repositories
{
    /// <summary>
    /// Represents repository for severity model.
    /// </summary>
    public class SeverityRepository : BaseRepository, ISeverityRepository<int, Severity>
    {
        /// <summary>
        /// Ctor for severity repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="tableName">Table name.</param>
        public SeverityRepository(IConfiguration configuration, string tableName = "Severities") 
            : base(configuration, tableName)
        { }

        /// <summary>
        /// Create severity.
        /// </summary>
        /// <param name="creationSeverity">Creation severity.</param>
        /// <returns>Return generated id for new severity.</returns>
        public async Task<int> CreateSeverityAsync(Severity creationSeverity)
        {
            var query = @$"INSERT INTO {TableName} (SeverityName, SeveriryDescription) 
                           VALUES (@SeverityName, @SeveriryDescription)
                           RETURNING Id";

            var parameters = new DynamicParameters();
            parameters.Add("SeverityName", creationSeverity.SeverityName, DbType.String);
            parameters.Add("SeveriryDescription", creationSeverity.SeveriryDescription, DbType.String);

            using var connection = OpenConnection();

            var createdId = await connection.ExecuteScalarAsync<int>(query, parameters);

            return createdId;
        }

        /// <summary>
        /// Delete severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns></returns>
        public async Task DeleteSeverityAsync(int id)
        {
            var query = @$"DELETE FROM {TableName} WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);

            using var connection = OpenConnection();

            var affectedRows = await connection.ExecuteAsync(query, parameters);

            if (affectedRows != 1)
                throw new InvalidOperationException("Delete operation don't execute.");
        }

        /// <summary>
        /// Get all severities.
        /// </summary>
        /// <returns>Returns collection of all severities.</returns>
        public async Task<IEnumerable<Severity>> GetAllSeveritiesAsync()
        {
            var query = @$"SELECT * FROM {TableName}";

            using var connection = OpenConnection();
            var severities = await connection.QueryAsync<Severity>(query);

            return severities.ToList();
        }

        /// <summary>
        /// Get all logs of sevetiry level.
        /// </summary>
        /// <param name="id">Severity level id.</param>
        /// <returns>Returns collection of severity level logs.</returns>
        public async Task<IEnumerable<Log>> GetAllSeverityLogs(int id)
        {
            var query = "SELECT * FROM Logs WHERE SeverityId = @SeverityId";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);

            using var connection = OpenConnection();
            var logs = await connection.QueryAsync<Log>(query, parameters);

            return logs;
        }

        /// <summary>
        /// Get severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Returns severity by given id.</returns>
        public async Task<Severity> GetSeverityAsync(int id)
        {
            var query = @$"SELECT * FROM {TableName} WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            using var connection = OpenConnection();
            var severity = await connection.QuerySingleOrDefaultAsync<Severity>(query);

            return severity;
        }

        /// <summary>
        /// Get sevetiry id by name.
        /// </summary>
        /// <param name="name">Severity name.</param>
        /// <returns>Returns severity id by given name. If severity with given name doesn't exists return -1.</returns>
        public async Task<int> GetSeverityIdByName(string name)
        {
            var query = @$"SELECT Id FROM {TableName}
                           WHERE SeverityName = @SeverityName";

            var parameters = new DynamicParameters();
            parameters.Add("SeverityName", name, DbType.String);

            using var connection = OpenConnection();
            var severity = await connection.QuerySingleOrDefaultAsync<Severity>(query);

            return severity is null 
                ? -1
                : severity.Id;
        }

        /// <summary>
        /// Get severity name by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Returns severity name by given id.</returns>
        public async Task<string> GetSeverityNameById(int id)
        {
            var query = @$"SELECT SeverityName FROM {TableName}
                           WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);

            using var connection = OpenConnection();
            var severity = await connection.QuerySingleOrDefaultAsync<Severity>(query);

            return severity?.SeverityName;
        }

        /// <summary>
        /// Update severity.
        /// </summary>
        /// <param name="updationSeverity">Updation severity.</param>
        /// <returns></returns>
        public async Task UpdateSeverityAsync(Severity updationSeverity)
        {
            var query = @$"UPDATE {TableName} 
                           SET Id = @Id, SeverityName = @SeverityName, SeveriryDescription = @SeveriryDescription
                           WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", updationSeverity.Id, DbType.Int32);
            parameters.Add("SeverityName", updationSeverity.SeverityName, DbType.String);
            parameters.Add("SeveriryDescription", updationSeverity.SeveriryDescription, DbType.String);

            using var connection = OpenConnection();

            var affectedRows = await connection.ExecuteAsync(query, parameters);

            if (affectedRows != 1)
                throw new InvalidOperationException("Update operation don't execute.");
        }
    }
}
