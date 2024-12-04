using Npgsql;

namespace StoreApiAuth.Services
{
    public class DatabaseStatusService
    {
        private readonly string _connectionString;

        public DatabaseStatusService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Dictionary<string, object>> GetDatabaseStatusAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var status = new Dictionary<string, object>();

            // Pega a versão do Servidor Postgres
            using (var command = new NpgsqlCommand("SHOW server_version;", connection))
            {
                var version = await command.ExecuteScalarAsync();
                status["version"] = version?.ToString() ?? "unknown";
            }

            // Informa as conexões abertas com o banco
            using (var command = new NpgsqlCommand("SELECT count(*)::int FROM pg_stat_activity WHERE datname = @dbName;", connection))
            {
                command.Parameters.AddWithValue("@dbName", connection.Database);

                var openConnections = await command.ExecuteScalarAsync();
                status["opened_connections"] = openConnections ?? 0;
            }

            // Obtém o número máximo de conexões
            using (var command = new NpgsqlCommand("SHOW max_connections;", connection))
            {
                var maxConnections = await command.ExecuteScalarAsync();
                status["max_connections"] = maxConnections?.ToString() ?? "unknown";
            }

            // Adiciona o horário da última consulta de status
            status["updated_at"] = DateTime.UtcNow.ToString("o");

            return status;
        }
    }
}
