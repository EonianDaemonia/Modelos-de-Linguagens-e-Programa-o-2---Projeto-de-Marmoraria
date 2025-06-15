CREATE TABLE IF NOT EXISTS Usuarios (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL,
    Login TEXT NOT NULL UNIQUE,
    Senha TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Blocos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoBloco TEXT NOT NULL UNIQUE,
    PedreiraOrigem TEXT,
    MetragemCubica REAL NOT NULL,
    TipoMaterial TEXT NOT NULL,
    ValorCompra REAL NOT NULL,
    NumeroNotaFiscal TEXT
);

CREATE TABLE IF NOT EXISTS Chapas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    BlocoOrigemId INTEGER,
    TipoMaterial TEXT NOT NULL,
    Altura REAL NOT NULL,
    Largura REAL NOT NULL,
    Valor REAL NOT NULL,
    FOREIGN KEY (BlocoOrigemId) REFERENCES Blocos(Id)
);

INSERT INTO Usuarios (Nome, Login, Senha) VALUES ('Administrador', 'admin', 'admin123');