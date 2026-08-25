namespace ServicoMLBEntidades.Application.Public;

public class PublicMedicaoResponse
{
    public DateOnly Data { get; set; }
    public string Etapa { get; set; } = string.Empty;
    public decimal? RecursosLiberados { get; set; }
}
