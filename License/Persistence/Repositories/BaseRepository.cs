using License.Persistence.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace License.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        private readonly SqlConnectionConfig sqlConfig;

        protected BaseRepository(IOptions<SqlConnectionConfig> sqlConfig)
        {
            this.sqlConfig = sqlConfig.Value;
        }

        protected SqlConnection GetConnection(bool isReadonly = false)
        {
            return new SqlConnection(isReadonly ? sqlConfig.ReadConnectionString : sqlConfig.WriteConnectionString);
        }

        protected async Task<int> ExecuteNonQueryAsync(string sql, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }

        protected async Task<object?> ExecuteScalarAsync(string sql, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync();
        }

        protected async Task<List<T>> ExecuteReaderAsync<T>(
            string sql,
            Func<SqlDataReader, T> map,
            List<SqlParameter>? parameters = null)
        {
            var list = new List<T>();

            using var connection = GetConnection(true);
            using var command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                list.Add(map(reader));

            return list;
        }
    }
}
