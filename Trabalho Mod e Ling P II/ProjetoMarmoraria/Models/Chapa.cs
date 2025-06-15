namespace ProjetoMarmoraria.Models;

public class Chapa
{
    public int Id { get; set; }
    public int? BlocoOrigemId { get; set; }
    public string? TipoMaterial { get; set; }
    public double Altura { get; set; }
    public double Largura { get; set; }
    public decimal Valor { get; set; }
}