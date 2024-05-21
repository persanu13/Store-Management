IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'Magazin2DB')
BEGIN
    DROP DATABASE Magazin2DB;
END;

CREATE DATABASE Magazin2DB;

USE Magazin2DB;
GO

--Creare tabele

CREATE TABLE Utilizatori (
    utilizator_id INT IDENTITY(1,1) PRIMARY KEY,
    nume_utilizator NVARCHAR(50) NOT NULL,
    parola NVARCHAR(50) NOT NULL,
    tip_utilizator VARCHAR(20) NOT NULL CHECK (tip_utilizator IN ('admin', 'cashier')),
    activ BIT NOT NULL
);

CREATE TABLE Categorii (
    categorie_id INT IDENTITY(1,1) PRIMARY KEY,
    nume_categorie NVARCHAR(50) NOT NULL,
    activ BIT NOT NULL
);

CREATE TABLE Producatori (
    producator_id INT IDENTITY(1,1) PRIMARY KEY,
    nume_producator NVARCHAR(50) NOT NULL,
    tara_origine NVARCHAR(50) NOT NULL,
    activ BIT NOT NULL
);

CREATE TABLE Produse (
    produs_id INT PRIMARY KEY IDENTITY(1,1),
    categorie_id INT NOT NULL,
    producator_id INT NOT NULL,
    nume_produs NVARCHAR(100) NOT NULL,
    cod_de_bare NVARCHAR(10) NOT NULL,
    activ BIT NOT NULL
);

CREATE TABLE Stocuri (
    stoc_id INT PRIMARY KEY IDENTITY(1,1),
    produs_id INT NOT NULL,
    cantitate INT NOT NULL,
    data_aprovizionare DATETIME NOT NULL,
    data_expirare DATETIME NOT NULL,
    pret_achizitie FLOAT NOT NULL,
    pret_vanzare FLOAT NOT NULL,
    activ BIT NOT NULL
);

CREATE TABLE Bonuri (
    bon_id INT PRIMARY KEY IDENTITY(1,1),
    utilizator_id INT NOT NULL,
	numar_bon INT NOT NULL,
    data_eliberare DATETIME NOT NULL,
    suma_totala FLOAT NOT NULL,
    activ BIT NOT NULL
);

CREATE TABLE BonProdus (
    bon_id INT NOT NULL,
    produs_id INT NOT NULL,
    cantitate INT NOT NULL,
    subtotal FLOAT NOT NULL,
    PRIMARY KEY (bon_id, produs_id)
);
GO

--Key straine

ALTER TABLE Produse
ADD CONSTRAINT FK_produs_categorie FOREIGN KEY (categorie_id) REFERENCES Categorii(categorie_id);

ALTER TABLE Produse
ADD CONSTRAINT FK_produs_producator FOREIGN KEY (producator_id) REFERENCES Producatori(producator_id);

ALTER TABLE Stocuri
ADD CONSTRAINT FK_stoc_produs  FOREIGN KEY (produs_id) REFERENCES Produse(produs_id);

ALTER TABLE Bonuri
ADD CONSTRAINT FK_bon_utilizator FOREIGN KEY (utilizator_id) REFERENCES Utilizatori(utilizator_id);

ALTER TABLE BonProdus
ADD CONSTRAINT FK_BonProdus_Bonuri FOREIGN KEY (bon_id) REFERENCES Bonuri(bon_id);

ALTER TABLE BonProdus
ADD CONSTRAINT FK_BonProdus_Produse FOREIGN KEY (produs_id) REFERENCES Produse(produs_id);
GO

--Proceduri Stocate Pentru Adaugari

CREATE PROCEDURE AdaugaUtilizator
    @nume_utilizator NVARCHAR(50),
    @parola NVARCHAR(100),
    @tip_utilizator VARCHAR(20),
    @utilizator_id INT OUTPUT
AS
BEGIN
    INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ)
    VALUES (@nume_utilizator, @parola, @tip_utilizator, 1);
    SET @utilizator_id = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE AdaugaCategorie
     @nume_categorie NVARCHAR(50),
     @categorie_id INT OUTPUT
AS
BEGIN
     INSERT INTO Categorii (nume_categorie, activ)
     VALUES (@nume_categorie, 1);
     SET @categorie_id = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE AdaugaProducator
    @nume_producator NVARCHAR(50),
    @tara_origine NVARCHAR(50),
	@producator_id INT OUTPUT
AS
BEGIN
    INSERT INTO Producatori (nume_producator, tara_origine, activ)
    VALUES (@nume_producator, @tara_origine, 1);
	SET @producator_id = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE AdaugaProdus
    @categorie_id INT,
    @producator_id INT,
    @nume_produs NVARCHAR(100),
    @cod_de_bare NVARCHAR(50),
    @produs_id INT OUTPUT
AS
BEGIN
    INSERT INTO Produse (categorie_id, producator_id, nume_produs, cod_de_bare, activ)
    VALUES (@categorie_id, @producator_id, @nume_produs, @cod_de_bare, 1);
    SET @produs_id = SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE AdaugaStoc
    @produs_id INT,
    @cantitate INT,
    @data_aprovizionare DATETIME,
    @data_expirare DATETIME,
    @pret_achizitie DECIMAL(18, 2),
    @pret_vanzare DECIMAL(18, 2),
	@stoc_id INT OUTPUT
AS
BEGIN
    INSERT INTO Stocuri (produs_id, cantitate, data_aprovizionare, data_expirare, pret_achizitie, pret_vanzare, activ)
    VALUES (@produs_id, @cantitate, @data_aprovizionare, @data_expirare, @pret_achizitie, @pret_vanzare, 1);
	SET @stoc_id = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE AdaugaBon
    @utilizator_id INT,
    @numar_bon INT,
    @data_eliberare DATETIME,
    @suma_totala FLOAT,
    @bon_id INT OUTPUT
AS
BEGIN
    INSERT INTO Bonuri (utilizator_id, numar_bon, data_eliberare, suma_totala, activ)
    VALUES (@utilizator_id, @numar_bon, @data_eliberare, @suma_totala, 1);
    SET @bon_id = SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE AdaugaBonProdus
    @bon_id INT,
    @produs_id INT,
    @cantitate INT,
    @subtotal FLOAT
AS
BEGIN
    INSERT INTO BonProdus (bon_id, produs_id, cantitate, subtotal)
    VALUES (@bon_id, @produs_id, @cantitate, @subtotal);
END;
GO

--Proceduri Stocate Pentru Stergere Logica

CREATE PROCEDURE StergereLogicaUtilizator
    @utilizator_id INT
AS
BEGIN
    UPDATE Utilizatori
    SET activ = 0
    WHERE utilizator_id = @utilizator_id;
END
GO

CREATE PROCEDURE StergereLogicaCategorie
    @categorie_id INT,
	@result BIT OUTPUT
AS
BEGIN
   IF EXISTS (
        SELECT 1 
        FROM Produse
        WHERE categorie_id = @categorie_id AND activ = 1
    )
    BEGIN
        SET @result = 0;
    END
    ELSE
    BEGIN
        UPDATE Categorii
        SET activ = 0
        WHERE categorie_id = @categorie_id;
        SET @result = 1;
    END
END
GO

CREATE PROCEDURE StergereLogicaProducator
    @producator_id INT,
	@result BIT OUTPUT
AS
BEGIN
     IF EXISTS (
        SELECT 1 
        FROM Produse
        WHERE producator_id = @producator_id AND activ = 1
    )
    BEGIN
        SET @result = 0;
    END
    ELSE
    BEGIN
        UPDATE Producatori
        SET activ = 0
        WHERE producator_id = @producator_id;
        SET @result = 1;
    END
END
GO

CREATE PROCEDURE StergereLogicaProdus
    @produs_id INT,
	@result BIT OUTPUT
AS
BEGIN
	IF EXISTS (
        SELECT 1 
        FROM Stocuri
        WHERE produs_id = @produs_id AND activ = 1
    )
    BEGIN
        SET @result = 0;
    END
    ELSE
	BEGIN
		UPDATE Produse
		SET activ = 0
		WHERE produs_id = @produs_id;
		SET @result = 1;
	END
END;
GO

CREATE PROCEDURE StergereLogicaStoc
    @stoc_id INT
AS
BEGIN
    UPDATE Stocuri
    SET activ = 0
    WHERE stoc_id = @stoc_id;
END
GO

CREATE PROCEDURE StergereLogicaBon
    @bon_id INT
AS
BEGIN
    UPDATE Bonuri
    SET activ = 0
    WHERE bon_id = @bon_id;
END;
GO

--Proceduri Stocate Pentru Actualizari

CREATE PROCEDURE ActualizeazaUtilizator
    @utilizator_id INT,
    @nume_utilizator NVARCHAR(50),
    @parola NVARCHAR(50),
    @tip_utilizator VARCHAR(20)
AS
BEGIN
    UPDATE Utilizatori
    SET nume_utilizator = @nume_utilizator,
        parola = @parola,
        tip_utilizator = @tip_utilizator
    WHERE utilizator_id = @utilizator_id;
END
GO

CREATE PROCEDURE ActualizeazaCategorie
    @categorie_id INT,
    @nume_categorie NVARCHAR(50)
AS
BEGIN
    UPDATE Categorii
    SET nume_categorie = @nume_categorie
    WHERE categorie_id = @categorie_id;
END
GO

CREATE PROCEDURE ActualizeazaProducator
    @producator_id INT,
    @nume_producator NVARCHAR(50),
    @tara_origine NVARCHAR(50)
AS
BEGIN
        UPDATE Producatori
        SET nume_producator = @nume_producator,
            tara_origine = @tara_origine
        WHERE producator_id = @producator_id;
END
GO

CREATE PROCEDURE ActualizeazaProdus
    @produs_id INT,
    @categorie_id INT,
    @producator_id INT,
    @nume_produs NVARCHAR(100),
    @cod_de_bare NVARCHAR(50)
AS
BEGIN
    UPDATE Produse
    SET
        categorie_id = @categorie_id,
        producator_id = @producator_id,
        nume_produs = @nume_produs,
        cod_de_bare = @cod_de_bare
    WHERE produs_id = @produs_id;
END
GO

CREATE PROCEDURE ActualizareCantitateStoc
    @stoc_id INT,
    @cantitate INT
AS
BEGIN
    UPDATE Stocuri
    SET 
        cantitate = @cantitate,
		activ = CASE WHEN @cantitate = 0 THEN 0 ELSE activ END
    WHERE 
        stoc_id = @stoc_id;
END
GO

--Proceduri Stocate Pentru Selecturi

CREATE PROCEDURE SelectUtilizatori
AS
BEGIN
    SELECT *
    FROM Utilizatori
    WHERE activ = 1;
END;
GO

CREATE PROCEDURE SelectCategorii
AS
BEGIN
    SELECT *
    FROM Categorii
    WHERE activ = 1;
END;
GO

CREATE PROCEDURE SelectProducatori
AS
BEGIN
    SELECT *
    FROM Producatori
    WHERE activ = 1;
END
GO

CREATE PROCEDURE SelectProduse
AS
BEGIN
    SELECT *
    FROM Produse
    WHERE activ = 1;
END;
GO

CREATE PROCEDURE SelectNumeCategorie
    @produs_id INT,
	@nume_categorie NVARCHAR(50) OUTPUT
AS
BEGIN
    SELECT @nume_categorie = c.nume_categorie
    FROM Produse p
    INNER JOIN Categorii c ON p.categorie_id = c.categorie_id
    WHERE p.produs_id = @produs_id;
END;
GO

CREATE PROCEDURE SelectNumeProducator
    @produs_id INT,
	@nume_producator NVARCHAR(50) OUTPUT
AS
BEGIN
    SELECT @nume_producator = Producatori.nume_producator
    FROM Produse p
    INNER JOIN Producatori ON p.producator_id = Producatori.producator_id
    WHERE p.produs_id = @produs_id;
END;
GO

CREATE PROCEDURE SelectNumeProdusById
    @produs_id INT,
    @nume_produs NVARCHAR(100) OUTPUT
AS
BEGIN
    SELECT @nume_produs = nume_produs
    FROM Produse
    WHERE produs_id = @produs_id;
END;
GO

CREATE PROCEDURE SelectStocuri
AS
BEGIN
    SELECT *
	FROM Stocuri
	WHERE activ = 1;
END
GO

CREATE PROCEDURE SelectProdusStoc
    @stoc_id INT
AS
BEGIN
    SELECT p.*
    FROM Stocuri s
    INNER JOIN Produse p ON s.produs_id = p.produs_id
    WHERE s.stoc_id = @stoc_id;
END;
GO

CREATE PROCEDURE SelectBonuri
AS
BEGIN
    SELECT * 
    FROM Bonuri
    WHERE activ = 1;
END;
GO

CREATE PROCEDURE SelectDetaliBon
		@bon_id INT
AS
BEGIN
    SELECT * 
    FROM  BonProdus
    WHERE bon_id = @bon_id;
END;
GO

CREATE PROCEDURE SelectNumeUtilizator
    @bon_id INT,
	@nume_utilizator NVARCHAR(50) OUTPUT
AS
BEGIN
    SELECT @nume_utilizator = u.nume_utilizator
    FROM Bonuri b
    JOIN Utilizatori u ON b.utilizator_id = u.utilizator_id
    WHERE b.bon_id = @bon_id;
END;
GO

CREATE PROCEDURE SelectMaxSumBonForDate
    @data_selectata DATETIME
AS
BEGIN
    SELECT TOP 1 *
    FROM Bonuri
    WHERE CAST(data_eliberare AS DATE) = CAST(@data_selectata AS DATE) AND activ = 1
    ORDER BY suma_totala DESC;
END;
GO

CREATE PROCEDURE SelectNumarBonuriCreateAstazi
    @numar_bonuri INT OUTPUT
AS
BEGIN
    DECLARE @data_curenta DATE;
    SET @data_curenta = CAST(GETDATE() AS DATE);
    SELECT @numar_bonuri = COUNT(*)
    FROM Bonuri
    WHERE CAST(data_eliberare AS DATE) = @data_curenta;
END;
GO

--Proceduri Stocate Pentru Filtrari

CREATE PROCEDURE FiltreazaUtilizatori
    @sir_caractere NVARCHAR(50),
    @tip_utilizator VARCHAR(20) = NULL
AS
BEGIN
    IF @tip_utilizator IS NULL
    BEGIN
        SELECT *
        FROM Utilizatori
        WHERE nume_utilizator LIKE '%' + @sir_caractere + '%'
        AND activ = 1;
    END
    ELSE
    BEGIN
        SELECT *
        FROM Utilizatori
        WHERE nume_utilizator LIKE '%' + @sir_caractere + '%'
        AND tip_utilizator = @tip_utilizator
        AND activ = 1;
    END
END
GO

CREATE PROCEDURE FiltreazaCategorii
    @sir_caractere NVARCHAR(100)
AS
BEGIN
     SELECT *
     FROM Categorii
     WHERE nume_categorie LIKE '%' + @sir_caractere + '%'
     AND activ = 1;
END
GO

CREATE PROCEDURE FiltreazaProducatori
    @sir_caractere NVARCHAR(50),
    @tara_origine NVARCHAR(50) = NULL
AS
BEGIN
    IF @tara_origine IS NOT NULL
    BEGIN
        SELECT *
        FROM Producatori
        WHERE nume_producator LIKE '%' + @sir_caractere + '%'
            AND tara_origine = @tara_origine
            AND activ = 1;
    END
    ELSE
    BEGIN
        SELECT *
        FROM Producatori
        WHERE nume_producator LIKE '%' + @sir_caractere + '%'
            AND activ = 1;
    END
END
GO

CREATE PROCEDURE FiltreazaProduse
    @sir NVARCHAR(100),
    @categorie_id INT = NULL,
    @producator_id INT = NULL
AS
BEGIN
    SELECT 
        produs_id,
        categorie_id,
        producator_id,
        nume_produs,
        cod_de_bare,
        activ
    FROM 
        Produse
    WHERE 
        activ = 1 AND
        nume_produs LIKE '%' + @sir + '%' AND
        (@categorie_id IS NULL OR categorie_id = @categorie_id) AND
        (@producator_id IS NULL OR producator_id = @producator_id);
END
GO

CREATE PROCEDURE FiltreazaStocuri
	@produs_id INT
AS
BEGIN
    SELECT * FROM Stocuri
	WHERE activ = 1 AND 
	(@produs_id IS NULL OR produs_id = @produs_id);
END
GO

CREATE PROCEDURE FiltrareStocuriPentruCasier
    @nume_produs NVARCHAR(100) = NULL,
    @cod_de_bare NVARCHAR(10) = NULL,
    @categorie_id INT = NULL,
    @producator_id INT = NULL,
    @data_expirare DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH FilteredStocks AS (
        SELECT 
            s.stoc_id,
            s.produs_id,
            s.cantitate,
            s.data_aprovizionare,
            s.data_expirare,
            s.pret_achizitie,
            s.pret_vanzare,
            s.activ,
            p.nume_produs,
            p.cod_de_bare,
            p.categorie_id,
            p.producator_id,
            ROW_NUMBER() OVER (PARTITION BY s.produs_id ORDER BY s.data_aprovizionare) AS rn
        FROM 
            Stocuri s
            INNER JOIN Produse p ON s.produs_id = p.produs_id
        WHERE 
            s.activ = 1
            AND (@nume_produs IS NULL OR p.nume_produs LIKE '%' + @nume_produs + '%')
            AND (@cod_de_bare IS NULL OR p.cod_de_bare LIKE @cod_de_bare + '%')
            AND (@categorie_id IS NULL OR p.categorie_id = @categorie_id)
            AND (@producator_id  IS NULL OR p.producator_id = @producator_id )
            AND (@data_expirare IS NULL OR s.data_expirare = @data_expirare)
    )
    SELECT 
        stoc_id,
        produs_id,
        cantitate,
        data_aprovizionare,
        data_expirare,
        pret_achizitie,
        pret_vanzare
    FROM 
        FilteredStocks
    WHERE 
        rn = 1;
END
GO

--Proceduri Stocate Pentru Verificari

CREATE PROCEDURE VerificaExistaNumeUtilizator
    @nume_utilizator NVARCHAR(50),
    @exista BIT OUTPUT
AS
BEGIN
    SET @exista = 0;
    IF EXISTS (SELECT 1 FROM Utilizatori WHERE nume_utilizator = @nume_utilizator AND activ = 1)
    BEGIN
        SET @exista = 1;
    END
END
GO

CREATE PROCEDURE VerificaExistaParolaUtilizator
    @parola NVARCHAR(50),
    @exista BIT OUTPUT
AS
BEGIN
    SET @exista = 0;
    IF EXISTS (SELECT 1 FROM Utilizatori WHERE parola = @parola AND activ = 1)
    BEGIN
        SET @exista = 1;
    END
END
GO

CREATE PROCEDURE VerificaExistaNumeCategorie
    @nume_categorie NVARCHAR(50),
    @exista BIT OUTPUT
AS
BEGIN
    SET @exista = 0;
    
    IF EXISTS (SELECT 1 FROM Categorii WHERE nume_categorie = @nume_categorie AND activ = 1)
    BEGIN
        SET @exista = 1;
    END
END;
GO

CREATE PROCEDURE VerificaExistaNumeProducator
    @nume_producator NVARCHAR(50),
    @exista BIT OUTPUT
AS
BEGIN
    SET @exista = 0;

    IF EXISTS (SELECT 1 FROM Producatori WHERE nume_producator = @nume_producator AND activ = 1)
    BEGIN
        SET @exista = 1;
    END
END
GO

CREATE PROCEDURE VerificaExistaCodDeBareProdus
    @cod_de_bare NVARCHAR(10),
    @exista BIT OUTPUT
AS
BEGIN
     SET @exista = 0;

    IF EXISTS (
        SELECT 1
        FROM Produse
        WHERE cod_de_bare = @cod_de_bare
        AND activ = 1
    )
    BEGIN
        SET @exista = 1;
    END
END
GO

--Inserti pentru teste in baza de date

-- Inserare date în tabelul Utilizatori
INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ) VALUES ('persanu13', 'parola12', 'admin', 1);
INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ) VALUES ('casier1', 'parola123', 'cashier', 1);
INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ) VALUES ('admin2', 'password789', 'admin', 1);
INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ) VALUES ('cashier2', 'password101', 'cashier', 1);

-- Inserare date in tabelul Categorii
INSERT INTO Categorii (nume_categorie, activ) VALUES ('Electronice', 1);
INSERT INTO Categorii (nume_categorie, activ) VALUES ('Alimente', 1);
INSERT INTO Categorii (nume_categorie, activ) VALUES ('Imbracaminte', 1);

-- Inserare date in tabelul Producatori
INSERT INTO Producatori (nume_producator, tara_origine, activ) VALUES ('Samsung', 'Coreea de Sud', 1);
INSERT INTO Producatori (nume_producator, tara_origine, activ) VALUES ('Nestle', 'Elvetia', 1);
INSERT INTO Producatori (nume_producator, tara_origine, activ) VALUES ('Nike', 'SUA', 1);

-- Inserare date in tabelul Produse
INSERT INTO Produse (categorie_id, producator_id, nume_produs, cod_de_bare, activ) VALUES (1, 1, 'Televizor', '1234567890', 1);
INSERT INTO Produse (categorie_id, producator_id, nume_produs, cod_de_bare, activ) VALUES (2, 2, 'Ciocolata', '2345678901', 1);
INSERT INTO Produse (categorie_id, producator_id, nume_produs, cod_de_bare, activ) VALUES (3, 3, 'Pantofi sport', '3456789012', 1);

-- Inserare date in tabelul Stocuri
INSERT INTO Stocuri (produs_id, cantitate, data_aprovizionare, data_expirare, pret_achizitie, pret_vanzare, activ) 
VALUES (1, 50, '2024-01-01', '2025-01-01', 1000.00, 1500.00, 1);
INSERT INTO Stocuri (produs_id, cantitate, data_aprovizionare, data_expirare, pret_achizitie, pret_vanzare, activ) 
VALUES (2, 200, '2024-02-01', '2025-02-01', 2.00, 3.00, 1);
INSERT INTO Stocuri (produs_id, cantitate, data_aprovizionare, data_expirare, pret_achizitie, pret_vanzare, activ) 
VALUES (3, 100, '2024-03-01', '2025-03-01', 50.00, 100.00, 1);

-- Inserare date in tabelul Bonuri
INSERT INTO Bonuri (utilizator_id, numar_bon, data_eliberare, suma_totala, activ) 
VALUES (1, 1001, '2024-04-01', 1500.00, 1);
INSERT INTO Bonuri (utilizator_id, numar_bon, data_eliberare, suma_totala, activ) 
VALUES (2, 1002, '2024-05-01', 600.00, 1);
INSERT INTO Bonuri (utilizator_id, numar_bon, data_eliberare, suma_totala, activ) 
VALUES (3, 1003, '2024-06-01', 5000.00, 1);

-- Inserare date in tabelul BonProdus
INSERT INTO BonProdus (bon_id, produs_id, cantitate, subtotal) VALUES (1, 1, 1, 1500.00);
INSERT INTO BonProdus (bon_id, produs_id, cantitate, subtotal) VALUES (2, 2, 200, 600.00);
INSERT INTO BonProdus (bon_id, produs_id, cantitate, subtotal) VALUES (3, 3, 50, 5000.00);

