using Microsoft.Data.SqlClient;

namespace StoreApiAuth.src.Services
{
    public class SqlServerStatusService
    {
        private readonly string _connectionString;

        public SqlServerStatusService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Dictionary<string, object>> GetDatabaseStatusAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var status = new Dictionary<string, object>();

            // Pega a versão do SQL Server
            using (var command = new SqlCommand("SELECT @@VERSION AS Version", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    status["version"] = reader["Version"];
                }
            }

            // Informa as conexões abertas com o banco
            using (var command = new SqlCommand(
                "SELECT COUNT(*) AS OpenedConnections FROM sys.dm_exec_sessions WHERE is_user_process = 1",
                connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    status["opened_connections"] = reader["OpenedConnections"];
                }
            }

            // Máximo de conexões
            using (var command = new SqlCommand(
                "SELECT value AS MaxConnections FROM sys.configurations WHERE name = 'user connections'",
                connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    status["max_connections"] = reader["MaxConnections"];
                }
            }

            // Adiciona o horário da última consulta de status
            status["updated_at"] = DateTime.UtcNow.ToString("o");

            return status;
        }
    }
}
