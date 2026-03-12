using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Products.DataAccess.DbContext;

internal class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string connectionString = _configuration.GetConnectionString("PostgresConnection")!;

        connectionString.Replace("$PG_HOST", Environment.GetEnvironmentVariable("PG_HOST"));
        connectionString.Replace("$PG_PASSWORD", Environment.GetEnvironmentVariable("PG_PASSWORD"));

        // Create connection
        _connection = new NpgsqlConnection(connectionString);
    }

    public IDbConnection DbConnection => _connection;
}
