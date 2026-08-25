using ServicoMLBEntidades.Domain.Models;

namespace ServicoMLBEntidades.Application.Mutirao;

public class PontuacaoFamiliaResponse
{
    public Guid FamiliaId { get; set; }
    public int PontuacaoAcumulada { get; set; }
    public bool BaixaParticipacao { get; set; }
    public List<PresencaResponse> Presencas { get; set; } = [];

    internal static PontuacaoFamiliaResponse FromResultado(PontuacaoFamiliaResultado resultado) => new()
    {
        FamiliaId = resultado.FamiliaId,
        PontuacaoAcumulada = resultado.PontuacaoAcumulada,
        BaixaParticipacao = resultado.BaixaParticipacao,
        Presencas = resultado.Presencas.Select(p => new PresencaResponse
        {
            Id = p.Id,
            MutiraoEscalaId = p.MutiraoEscalaId,
            FamiliaId = p.FamiliaId,
            DataRegistro = p.DataRegistro,
            PontuacaoConcedida = p.PontuacaoConcedida,
        }).ToList(),
    };
}
