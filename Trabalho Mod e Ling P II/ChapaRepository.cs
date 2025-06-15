using ProjetoMarmoraria.Models;
using System.Data.SQLite;

namespace ProjetoMarmoraria.Data;

public class ChapaRepository
{
    public void Cadastrar(Chapa chapa)
    {
        using (var connection = Database.GetConnection())
        {
            var command = new SQLiteCommand(
                "INSERT INTO Chapas (BlocoOrigemId, TipoMaterial, Altura, Largura, Valor) " +
                "VALUES (@blocoId, @tipo, @altura, @largura, @valor)", connection);
            command.Parameters.AddWithValue("@blocoId", chapa.BlocoOrigemId);
            command.Parameters.AddWithValue("@tipo", chapa.TipoMaterial);
            command.Parameters.AddWithValue("@altura", chapa.Altura);
            command.Parameters.AddWithValue("@largura", chapa.Largura);
            command.Parameters.AddWithValue("@valor", chapa.Valor);
            command.ExecuteNonQuery();
        }
    }

    public List<Chapa> Listar()
    {
        var chapas = new List<Chapa>();
        using (var connection = Database.GetConnection())
        {
            var command = new SQLiteCommand("SELECT * FROM Chapas", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chapas.Add(new Chapa
                    {
                        Id = reader.GetInt32(0),
                        BlocoOrigemId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                        TipoMaterial = reader.GetString(2),
                        Altura = reader.GetDouble(3),
                        Largura = reader.GetDouble(4),
                        Valor = reader.GetDecimal(5)
                    });
                }
            }
        }
        return chapas;
    }
}