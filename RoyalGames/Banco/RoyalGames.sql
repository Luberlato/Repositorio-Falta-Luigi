CREATE DATABASE RoyalGames;
GO

USE RoyalGames;
GO

CREATE TABLE Usuario(
	UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(60) NOT NULL,
	Email VARCHAR(150) UNIQUE,
	Senha VARBINARY(32) NOT NULL,
	StatusUsuario BIT DEFAULT 1
);
GO

CREATE TABLE ClassificacaoIndicativa(
ClassificacaoIndicativaID INT IDENTITY PRIMARY KEY,
Faixa VARCHAR(5) NOT NULL
);

CREATE TABLE Jogo(
	JogoID INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(60) NOT NULL,
	Descricao NVARCHAR(MAX) NOT NULL,
	Preco DECIMAL(10,2) NOT NULL,
	Imagem VARBINARY(MAX) NOT NULL,
	DataLancamento DATETIME2(0),
	StatusJogo BIT,

	ClassificacaoIndicativaID INT FOREIGN KEY REFERENCES ClassificacaoIndicativa(ClassificacaoIndicativaID) NOT NULL,
	UsuarioID INT FOREIGN KEY REFERENCES Usuario(UsuarioID) NOT NULL
	);
GO

CREATE TABLE Genero(
	GeneroID INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(30) NOT NULL
);
GO

CREATE TABLE Plataforma(
	PlataformaID INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(60) NOT NULL
);
GO

CREATE TABLE Log_AlteracaoJogo(
	AlteracaoID INT IDENTITY PRIMARY KEY,
	DataAlteracao DATETIME2(0),
	NomeAnterior VARCHAR(60),
	PrecoAnterior DECIMAL(10,2),

	JogoID INT FOREIGN KEY (JogoID) REFERENCES Jogo(JogoID) NOT NULL
);
GO

-- TABELAS INTERMEDIARIAS

CREATE TABLE JogoGenero(
	JogoID INT NOT NULL,
	GeneroID INT NOT NULL,

	CONSTRAINT Pk_JogoGenero PRIMARY KEY (JogoID, GeneroID),
	CONSTRAINT Fk_JogoGenero_Jogo FOREIGN KEY (JogoID) REFERENCES Jogo(JogoID) ON DELETE CASCADE,
	CONSTRAINT Fk_JogoGenero_Genero FOREIGN KEY (GeneroID) REFERENCES Genero(GeneroID) ON DELETE CASCADE
)
GO

CREATE TABLE JogoPlataforma(
	JogoID INT NOT NULL,
	PlataformaID INT NOT NULL,

	CONSTRAINT Pk_JogoPlataforma PRIMARY KEY (JogoID, PlataformaID),
	CONSTRAINT Fk_JogoPlataforma_Jogo FOREIGN KEY (JogoID) REFERENCES Jogo(JogoID) ON DELETE CASCADE, 
	CONSTRAINT Fk_JogoPlataforma_Plataforma FOREIGN KEY (PlataformaID) REFERENCES Plataforma(PlataformaID) ON DELETE CASCADE
)
GO

-- TRIGGERS

CREATE TRIGGER trg_LogAlteracaoJogos
ON Jogo
AFTER UPDATE 
AS BEGIN 
	INSERT INTO Log_AlteracaoJogo(DataAlteracao, JogoID, NomeAnterior, PrecoAnterior) 
	SELECT GETDATE(), JogoID, Nome, Preco FROM deleted
	END
GO


CREATE TRIGGER trg_ExclusaoJogo
ON Jogo
INSTEAD OF DELETE
AS BEGIN
	UPDATE j SET StatusJogo = 0
	FROM Jogo j 
	INNER JOIN deleted d 
		ON d.JogoID = j.JogoID
	END
GO

CREATE TRIGGER trg_ExclusaoUsuario
ON Usuario
INSTEAD OF DELETE
AS BEGIN
	UPDATE u SET StatusUsuario = 0
	FROM Usuario u 
	INNER JOIN deleted d 
		ON d.UsuarioID = u.UsuarioID
	END
GO

-- INSERTS

-- Inserindo Usuários
INSERT INTO Usuario (Nome, Email, Senha, StatusUsuario) VALUES 
('Arthur Pendragon', 'arthur@royal.com', 0x5468697349734153656372657448617368, 1),
('Guinevere Queen', 'guine@royal.com', 0x416e6f7468657253656372657448617368, 1),
('Merlin Wizard', 'merlin@royal.com', 0x4d6167696350617373776f726431323334, 1);
GO

-- Inserindo Classificação Indicativa
INSERT INTO ClassificacaoIndicativa (Faixa) VALUES 
('L'), 
('14'), 
('18');
GO

-- Inserindo Gêneros (Nota: ajuste o tamanho do VARCHAR se necessário no seu DB)
INSERT INTO Genero (Nome) VALUES 
('RPG'), 
('Ação'), 
('Estratégia');
GO

-- Inserindo Plataformas
INSERT INTO Plataforma (Nome) VALUES 
('PC'), 
('PS5'), 
('Xbox Series X');
GO

INSERT INTO Jogo (Nome, Descricao, Preco, Imagem, DataLancamento, StatusJogo, ClassificacaoIndicativaID, UsuarioID) VALUES 
('Quest of Kings', 'Um RPG épico medieval.', 199.90, 0xFFD8FFE0, '2024-01-15', 1, 1, 1),
('Shadow Strike', 'Jogo de ação stealth em primeira pessoa.', 150.00, 0xFFD8FFE0, '2023-11-20', 1, 2, 2),
('Empire Builder', 'Gerencie seu reino em tempo real.', 89.90, 0xFFD8FFE0, '2025-02-05', 1, 1, 3);
GO

-- Relacionando Jogos e Gêneros
INSERT INTO JogoGenero (JogoID, GeneroID) VALUES 
(1, 1), -- Quest of Kings é RPG
(2, 2), -- Shadow Strike é Ação
(3, 3); -- Empire Builder é Estratégia
GO

-- Relacionando Jogos e Plataformas
INSERT INTO JogoPlataforma (JogoID, PlataformaID) VALUES 
(1, 1), -- Quest of Kings no PC
(1, 2), -- Quest of Kings no PS5
(2, 3); -- Shadow Strike no Xbox
GO

