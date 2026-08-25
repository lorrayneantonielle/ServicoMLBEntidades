using Dapper;
using Microsoft.Extensions.Configuration;
using ServicoMLBEntidades.Domain.Enums;
using ServicoMLBEntidades.Domain.Services;

namespace ServicoMLBEntidades.Application.Public;

public class PublicService
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IConfiguration _configuration;

    public PublicService(IDbConnectionFactory connectionFactory, IConfiguration configuration)
    {
        _connectionFactory = connectionFactory;
        _configuration = configuration;
    }

    public async Task<PublicStatusResponse> GetStatusAsync(CancellationToken ct = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(ct);

        const string sql = "SELECT COALESCE(AVG(percentual_conclusao), 0) FROM etapas_obra";
        var percentual = await connection.ExecuteScalarAsync<decimal>(new CommandDefinition(sql, cancellationToken: ct));

        return new PublicStatusResponse
        {
            NomeEmpreendimento = _configuration["Empreendimento:Nome"] ?? "Empreendimento MLBEntidades",
            Descricao = _configuration["Empreendimento:Descricao"]
                ?? "Acompanhamento público do empreendimento habitacional do movimento social.",
            PercentualConclusaoGeral = percentual,
        };
    }

    public async Task<List<PublicEtapaResponse>> GetEtapasAsync(CancellationToken ct = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(ct);

        const string sql = """
            SELECT nome AS "Nome", ordem AS "Ordem", percentual_conclusao AS "PercentualConclusao"
            FROM etapas_obra
            ORDER BY ordem
            """;

        var etapas = await connection.QueryAsync<PublicEtapaResponse>(new CommandDefinition(sql, cancellationToken: ct));
        return etapas.ToList();
    }

    public async Task<List<PublicMedicaoResponse>> GetMedicoesAprovadasAsync(CancellationToken ct = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(ct);

        const string sql = """
            SELECT m.data AS "Data", e.nome AS "Etapa", m.recursos_liberados AS "RecursosLiberados"
            FROM medicoes m
            JOIN etapas_obra e ON e.id = m.etapa_obra_id
            WHERE m.status_aprovacao = 'Aprovada'
            ORDER BY m.data
            """;

        var medicoes = await connection.QueryAsync<PublicMedicaoResponse>(new CommandDefinition(sql, cancellationToken: ct));
        return medicoes.ToList();
    }

    private class MutiraoRow
    {
        public DateOnly Data { get; set; }
        public string Turno { get; set; } = string.Empty;
        public int VagasDisponiveis { get; set; }
    }

    public async Task<List<PublicMutiraoResponse>> GetProximosMutiroesAsync(CancellationToken ct = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(ct);

        const string sql = """
            SELECT me.data AS "Data", me.turno AS "Turno",
                   (me.vagas_totais - COUNT(p.id)) AS "VagasDisponiveis"
            FROM mutirao_escalas me
            LEFT JOIN presencas p ON p.mutirao_escala_id = me.id
            WHERE me.data >= CURRENT_DATE
            GROUP BY me.id, me.data, me.turno, me.vagas_totais
            ORDER BY me.data
            """;

        var rows = await connection.QueryAsync<MutiraoRow>(new CommandDefinition(sql, cancellationToken: ct));

        return rows.Select(r => new PublicMutiraoResponse
        {
            Data = r.Data,
            Turno = Enum.Parse<Turno>(r.Turno),
            VagasDisponiveis = r.VagasDisponiveis,
        }).ToList();
    }
}
