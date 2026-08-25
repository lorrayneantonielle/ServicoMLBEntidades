using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Infrastructure.Persistence;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Configuração 'ConnectionStrings:Default' não encontrada.");
    }

    public DbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
