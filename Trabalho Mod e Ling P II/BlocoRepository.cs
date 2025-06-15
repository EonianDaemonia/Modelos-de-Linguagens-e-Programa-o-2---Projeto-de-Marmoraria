using ProjetoMarmoraria.Models;
using System.Data.SQLite;

namespace ProjetoMarmoraria.Data;

public class BlocoRepository
{
    public void Cadastrar(Bloco bloco)
    {
        using (var connection = Database.GetConnection())
        {
            var command = new SQLiteCommand(
                "INSERT INTO Blocos (CodigoBloco, PedreiraOrigem, MetragemCubica, TipoMaterial, ValorCompra, NumeroNotaFiscal) " +
                "VALUES (@codigo, @pedreira, @metragem, @tipo, @valor, @nota)", connection);
            command.Parameters.AddWithValue("@codigo", bloco.CodigoBloco);
            command.Parameters.AddWithValue("@pedreira", bloco.PedreiraOrigem);
            command.Parameters.AddWithValue("@metragem", bloco.MetragemCubica);
            command.Parameters.AddWithValue("@tipo", bloco.TipoMaterial);
            command.Parameters.AddWithValue("@valor", bloco.ValorCompra);
            command.Parameters.AddWithValue("@nota", bloco.NumeroNotaFiscal);
            command.ExecuteNonQuery();
        }
    }

    public List<Bloco> Listar()
    {
        var blocos = new List<Bloco>();
        using (var connection = Database.GetConnection())
        {
            var command = new SQLiteCommand("SELECT * FROM Blocos", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    blocos.Add(new Bloco
                    {
                        Id = reader.GetInt32(0),
                        CodigoBloco = reader.GetString(1),
                        PedreiraOrigem = reader.GetString(2),
                        MetragemCubica = reader.GetDouble(3),
                        TipoMaterial = reader.GetString(4),
                        ValorCompra = reader.GetDecimal(5),
                        NumeroNotaFiscal = reader.GetString(6)
                    });
                }
            }
        }
        return blocos;
    }
}