using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ERPMateus.Services;

public sealed class SqlConnectionTester
{
    public async Task<(bool Ok, string Message)> TestAsync(
        string server, string database, string user, string password,
        CancellationToken ct = default)
    {
        try
        {
            var cs = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                UserID = user,
                Password = password,
                TrustServerCertificate = true,
                Encrypt = true,
                ConnectTimeout = 5
            }.ConnectionString;

            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            var r = await cmd.ExecuteScalarAsync(ct);

            return (true, "Conexão OK.");
        }
        catch (Exception ex)
        {
            return (false, $"Falha: {ex.Message}");
        }
    }
}