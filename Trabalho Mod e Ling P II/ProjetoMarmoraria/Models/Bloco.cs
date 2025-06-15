namespace ProjetoMarmoraria.Models;

public class Bloco
{
    public int Id { get; set; }
    public string? CodigoBloco { get; set; }
    public string? PedreiraOrigem { get; set; }
    public double MetragemCubica { get; set; }
    public string? TipoMaterial { get; set; }
    public decimal ValorCompra { get; set; }
    public string? NumeroNotaFiscal { get; set; }
}