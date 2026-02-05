CREATE DATABASE VH_Burguer;
GO

USE VH_Burguer;
GO

CREATE TABLE Usuario(
	UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(60) NOT NULL,
	Email VARCHAR(150) UNIQUE NOT NULL,
	Senha VARBINARY(32),
	StatusUsuario BIT DEFAULT 1
);
GO

CREATE TABLE Produto(
	ProdutoID INT IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL UNIQUE,
	Preco DECIMAL(10,2) NOT NULL,
	Descricao NVARCHAR(MAX) NOT NULL,
	Imagem VARBINARY(MAX) NOT NULL,
	StatusProduto BIT DEFAULT 0,

	UsuarioID INT FOREIGN KEY REFERENCES Usuario(UsuarioID) 
	);
GO

CREATE TABLE Categoria(
	CategoriaID INT PRIMARY KEY IDENTITY (1,1),
	Nome VARCHAR(60) NOT NULL
	);
GO

CREATE TABLE ProdutoCategoria(
	ProdutoID INT NOT NULL,
	CategoriaID INT NOT NULL,

	CONSTRAINT Pk_ProdutoCategoria PRIMARY KEY (ProdutoID, CategoriaID),
	CONSTRAINT Fk_ProdutoCategoria_Produto FOREIGN KEY (ProdutoID) REFERENCES Produto(ProdutoID) ON DELETE CASCADE,
	CONSTRAINT Fk_ProdutoCategoria_Categoria FOREIGN KEY (CategoriaID) REFERENCES Categoria(CategoriaID) ON DELETE CASCADE
	);
GO

CREATE TABLE Promocao(
	PromocaoID INT IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(100),
	DataExpiracao DATETIME2(0) NOT NULL,
	StatusPromocao BIT DEFAULT 1 NOT NULL
	);
GO

CREATE TABLE ProdutoPromocao(
	ProdutoID INT NOT NULL,
	PromocaoID INT NOT NULL,
	PrecoAtual DECIMAL(10,2) NOT NULL,

	CONSTRAINT Pk_ProdutoPromocao PRIMARY KEY (ProdutoID, PromocaoID),
	CONSTRAINT Fk_ProdutoPromocao_Produto FOREIGN KEY (ProdutoID) REFERENCES Produto(ProdutoID) ON DELETE CASCADE,
	CONSTRAINT Fk_ProdutoPromocao_Promocao FOREIGN KEY (PromocaoID) REFERENCES Promocao(PromocaoID) ON DELETE CASCADE
	);
GO

CREATE TABLE Log_AlteracaoProduto(
	Log_AlteracaoProduto INT PRIMARY KEY IDENTITY,
	DataAlteracao DATETIME2(0) NOT NULL,
	NomeAnterior VARCHAR(100),
	ValorAnterior DECIMAL(10,2),
	
	ProdutoID INT FOREIGN KEY REFERENCES Produto(ProdutoID)
)
GO


-- Criando as triggers
	-- DELETE -> EXCLUIR O USUARIO = INATIVAR O USUARIO: StatusUsuario = 0
	CREATE TRIGGER trg_ExclusaoUsuario
	ON Usuario
	-- Inves de deletar um usuario, ocorrera um update no usario o desativando
	INSTEAD OF DELETE
	AS BEGIN
		UPDATE usr SET StatusUsuario = 0
		FROM Usuario usr 
		INNER JOIN deleted d 
			ON d.UsuarioID = usr.UsuarioID
	END
	GO

	-- Toda vez que alterarmos a tabela produto = Cria um regitro na tabela Log
	CREATE TRIGGER trg_LogAlteracaoProduto
	On Produto
	AFTER UPDATE
	AS BEGIN
		INSERT INTO Log_AlteracaoProduto(DataAlteracao, ProdutoId, NomeAnterior, ValorAnterior)
		SELECT GETDATE(), ProdutoID, Nome, Preco FROM deleted
	END
	GO

	-- Criando as triggers
	-- DELETE -> EXCLUIR O USUARIO = INATIVAR O USUARIO: StatusUsuario = 0
	CREATE TRIGGER trg_ExclusaoProduto
	ON Produto
	-- Inves de deletar um usuario, ocorrera um update no usario o desativando
	INSTEAD OF DELETE
	AS BEGIN
		UPDATE p SET StatusProduto = 0
		FROM Produto p 
		INNER JOIN deleted d 
			ON d.ProdutoID = p.ProdutoID
	END
	GO

	--- DML
INSERT INTO Usuario (Nome, Email, Senha)
	VALUES 
	('Carlos Lima', 'carlos@vhburguer.com', HASHBYTES('SHA2_256', 'admin@123'));
GO



INSERT INTO Categoria (Nome)
	VALUES
	('Vegetariano'),
	('Vegano'),
	('Especial');

GO


INSERT INTO Produto (Nome, Preco, Descricao, Imagem, UsuarioID)
VALUES
('VH Classic Burger', 29.90, 'Hamburguer artesanal com pão brioche, carne e queijo cheddar.', CONVERT(VARBINARY(MAX), 'imagem aleatoria'), 1),
('VH Bacon Supreme', 34.90, 'Hambúrguer bovino, bacon crocante, queijo e molho especial da casa.', CONVERT(VARBINARY(MAX), 'imagem aleatoria'), 1),
('Batata Rústica', 14.90, 'Batatas rústicas temperadas com ervas finas.', CONVERT(VARBINARY(MAX), 'imagem aleatoria'), 1);

GO

INSERT INTO ProdutoCategoria (ProdutoID, CategoriaID)
VALUES
(1, 3), 
(2, 3), 
(3, 1),
(3, 3),
(3, 2);

GO

INSERT INTO Promocao (Nome, DataExpiracao)
VALUES
('Promoção Semana do Hambúrguer', '2026-03-01 23:59:59'),
('Combo Happy Hour', '2026-02-20 23:59:59');
GO

INSERT INTO ProdutoPromocao (ProdutoID, PromocaoID, PrecoAtual)
VALUES
(1, 1, 24.90), -- VH Classic Burger
(2, 1, 29.90), -- VH Bacon Supreme
(3, 2, 9.90); -- Batata Rústica
GO

SELECT * FROM Produto
SELECT * FROM Promocao
SELECT * FROM ProdutoPromocao
