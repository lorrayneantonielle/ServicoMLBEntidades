using Dapper;
using ServicoMLBEntidades.Domain.Models;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Infrastructure.Queries;

public class PontuacaoQueryService : IPontuacaoQueryService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PontuacaoQueryService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private class PontuacaoRow
    {
        public Guid FamiliaId { get; set; }
        public int PontuacaoAcumulada { get; set; }
        public Guid? PresencaId { get; set; }
        public Guid? MutiraoEscalaId { get; set; }
        public DateTimeOffset? DataRegistro { get; set; }
        public int? PontuacaoConcedida { get; set; }
    }

    public async Task<List<PontuacaoFamiliaResultado>> ObterPontuacaoPorFamiliaAsync(Guid? familiaId, CancellationToken ct = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(ct);

        const string sqlLimite = "SELECT limite_minimo_pontuacao_mutirao FROM configuracao_sistema LIMIT 1";
        var limiteMinimo = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlLimite, cancellationToken: ct));

        const string sql = """
            SELECT f.id AS "FamiliaId", f.pontuacao_acumulada AS "PontuacaoAcumulada",
                   p.id AS "PresencaId", p.mutirao_escala_id AS "MutiraoEscalaId",
                   p.data_registro AS "DataRegistro", p.pontuacao_concedida AS "PontuacaoConcedida"
            FROM familias f
            LEFT JOIN presencas p ON p.familia_id = f.id
            WHERE f.excluida = false
              AND (@familiaId::uuid IS NULL OR f.id = @familiaId)
            ORDER BY f.id, p.data_registro
            """;

        var rows = await connection.QueryAsync<PontuacaoRow>(
            new CommandDefinition(sql, new { familiaId }, cancellationToken: ct));

        return rows
            .GroupBy(r => r.FamiliaId)
            .Select(g => new PontuacaoFamiliaResultado
            {
                FamiliaId = g.Key,
                PontuacaoAcumulada = g.First().PontuacaoAcumulada,
                BaixaParticipacao = g.First().PontuacaoAcumulada < limiteMinimo,
                Presencas = g
                    .Where(r => r.PresencaId.HasValue)
                    .Select(r => new PresencaResultado
                    {
                        Id = r.PresencaId!.Value,
                        MutiraoEscalaId = r.MutiraoEscalaId!.Value,
                        FamiliaId = g.Key,
                        DataRegistro = r.DataRegistro!.Value,
                        PontuacaoConcedida = r.PontuacaoConcedida!.Value,
                    })
                    .ToList(),
            })
            .ToList();
    }
}
