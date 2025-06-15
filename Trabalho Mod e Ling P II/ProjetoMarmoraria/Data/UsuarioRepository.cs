using ProjetoMarmoraria.Models;
using System.Data.SQLite;

namespace ProjetoMarmoraria.Data;

public class UsuarioRepository
{
    public Usuario? ValidarLogin(string login, string senha)
    {
        using (var connection = Database.GetConnection())
        {
            var command = new SQLiteCommand("SELECT * FROM Usuarios WHERE Login = @login AND Senha = @senha", connection);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@senha", senha);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Login = reader.GetString(2)
                    };
                }
            }
        }
        return null;
    }

    public void Cadastrar(Usuario usuario)
    {
        using (var connection = Database.GetConnection())
        {
            var command = new SQLiteCommand("INSERT INTO Usuarios (Nome, Login, Senha) VALUES (@nome, @login, @senha)", connection);
            command.Parameters.AddWithValue("@nome", usuario.Nome);
            command.Parameters.AddWithValue("@login", usuario.Login);
            command.Parameters.AddWithValue("@senha", usuario.Senha);
            command.ExecuteNonQuery();
        }
    }
}