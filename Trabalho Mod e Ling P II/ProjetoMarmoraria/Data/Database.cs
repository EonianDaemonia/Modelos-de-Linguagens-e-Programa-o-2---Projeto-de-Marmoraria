using System.Data.SQLite;

namespace ProjetoMarmoraria.Data;

public static class Database
{
    private static readonly string _connectionString = "Data Source=MarmorariaDB.sqlite";

    public static SQLiteConnection GetConnection()
    {
        var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public static void Initialize()
    {
        if (File.Exists("MarmorariaDB.sqlite")) return;
        
        SQLiteConnection.CreateFile("MarmorariaDB.sqlite");
        using (var connection = GetConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE Usuarios (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT NOT NULL, Login TEXT NOT NULL UNIQUE, Senha TEXT NOT NULL
                    );
                    CREATE TABLE Blocos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, CodigoBloco TEXT NOT NULL UNIQUE, PedreiraOrigem TEXT,
                        MetragemCubica REAL NOT NULL, TipoMaterial TEXT NOT NULL, ValorCompra REAL NOT NULL, NumeroNotaFiscal TEXT
                    );
                    CREATE TABLE Chapas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT, BlocoOrigemId INTEGER, TipoMaterial TEXT NOT NULL,
                        Altura REAL NOT NULL, Largura REAL NOT NULL, Valor REAL NOT NULL,
                        FOREIGN KEY (BlocoOrigemId) REFERENCES Blocos(Id)
                    );
                    INSERT INTO Usuarios (Nome, Login, Senha) VALUES ('Administrador', 'admin', 'admin123');
                ";
                command.ExecuteNonQuery();
            }
        }
    }
}