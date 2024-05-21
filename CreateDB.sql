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

CREATE PROCEDURE SelectSumaTotalaByCategory
    @categorie_id INT,
    @total_sum FLOAT OUTPUT
AS
BEGIN
    SET @total_sum = 0;
    SELECT @total_sum = CAST(ISNULL(SUM(Stocuri.pret_vanzare * Stocuri.cantitate), 0)  AS FLOAT)
    FROM Produse
    JOIN Stocuri ON Produse.produs_id = Stocuri.produs_id
    WHERE Produse.categorie_id = @categorie_id
		  AND Stocuri.activ = 1;
END;
GO

CREATE PROCEDURE SelectSumaTotalaZilnicaByUtilizator
    @utilizator_id INT,
    @data_calendaristica DATE
AS
BEGIN
    DECLARE @prima_zi DATE;
    DECLARE @ultima_zi DATE;

    SET @prima_zi = DATEFROMPARTS(YEAR(@data_calendaristica), MONTH(@data_calendaristica), 1);
    SET @ultima_zi = EOMONTH(@data_calendaristica);

    DECLARE @zile TABLE (zi DATE);

    DECLARE @zi DATE = @prima_zi;
    WHILE @zi <= @ultima_zi
    BEGIN
        INSERT INTO @zile VALUES (@zi);
        SET @zi = DATEADD(DAY, 1, @zi);
    END

    SELECT 
        z.zi AS ziua,
        CAST(ISNULL(SUM(b.suma_totala), 0) AS FLOAT) AS total_zi
    FROM 
        @zile z
    LEFT JOIN 
        Bonuri b ON z.zi = CAST(b.data_eliberare AS DATE) AND b.utilizator_id = @utilizator_id
    GROUP BY 
        z.zi
    ORDER BY 
        z.zi;
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

-- Agent pentru dezactivare automata stocuri expirate


--Creare Procedura Stocata pe care o utilizeaza Jobul

CREATE PROCEDURE DezactiveazaStocuriExpirate
AS
BEGIN
    UPDATE Stocuri
    SET activ = 0
    WHERE data_expirare < GETDATE() AND activ = 1;
END;
GO

USE msdb;
GO

-- Creare Job
EXEC dbo.sp_add_job
    @job_name = N'DezactiveazaStocuriExpirateJob';
GO

-- Adauga pasul pentru job
EXEC dbo.sp_add_jobstep
    @job_name = N'DezactiveazaStocuriExpirateJob',
    @step_name = N'Step1',
    @subsystem = N'TSQL',
    @command = N'EXEC Magazin2DB.dbo.DezactiveazaStocuriExpirate;',
    @retry_attempts = 5,
    @retry_interval = 5;
GO

-- Programeaza job-ul sa ruleze zilnic la ora 00:01
EXEC dbo.sp_add_schedule
    @schedule_name = N'DailySchedule',
    @freq_type = 4,
    @freq_interval = 1,
    @active_start_time = 000101; -- Ora 00:01
GO

-- Ataseaza programul la job
EXEC dbo.sp_attach_schedule
    @job_name = N'DezactiveazaStocuriExpirateJob',
    @schedule_name = N'DailySchedule';
GO

-- Activeaza job-ul
EXEC dbo.sp_add_jobserver
    @job_name = N'DezactiveazaStocuriExpirateJob';

USE Magazin2DB;
GO

-- Inserti pentru teste in baza de date

-- Inserare date in tabelul Utilizatori
INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ) VALUES ('persanu13', 'parola12', 'admin', 1);
INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ) VALUES ('casier1', 'parola123', 'cashier', 1);

INSERT INTO Utilizatori (nume_utilizator, parola, tip_utilizator, activ)
VALUES 
('admin1', 'password1', 'admin', 1),
('admin2', 'password2', 'admin', 1),
('admin3', 'password3', 'admin', 1),
('admin4', 'password4', 'admin', 1),
('admin5', 'password5', 'admin', 1),
('cashier1', 'password6', 'cashier', 1),
('cashier2', 'password7', 'cashier', 1),
('cashier3', 'password8', 'cashier', 1),
('cashier4', 'password9', 'cashier', 1),
('cashier5', 'password10', 'cashier', 1),
('cashier6', 'password11', 'cashier', 1),
('cashier7', 'password12', 'cashier', 1),
('cashier8', 'password13', 'cashier', 1),
('cashier9', 'password14', 'cashier', 1),
('cashier10', 'password15', 'cashier', 1),
('admin6', 'password16', 'admin', 1),
('admin7', 'password17', 'admin', 1),
('admin8', 'password18', 'admin', 1),
('admin9', 'password19', 'admin', 1),
('admin10', 'password20', 'admin', 1);

-- Inserare date in tabelul Categorii
INSERT INTO Categorii (nume_categorie, activ)
VALUES 
('Electronics', 1),
('Food', 1),
('Clothing', 1),
('Books', 1),
('Furniture', 1),
('Toys', 1),
('Tools', 1),
('Sports', 1),
('Beauty', 1),
('Health', 1),
('Automotive', 1),
('Jewelry', 1),
('Garden', 1),
('Pets', 1),
('Office Supplies', 1),
('Music', 1),
('Movies', 1),
('Games', 1),
('Software', 1),
('Shoes', 1);

-- Inserare date in tabelul Producatori
INSERT INTO Producatori (nume_producator, tara_origine, activ)
VALUES 
('Apple', 'USA', 1),
('Sony', 'Japan', 1),
('LG', 'South Korea', 1),
('Dell', 'USA', 1),
('HP', 'USA', 1),
('Asus', 'Taiwan', 1),
('Lenovo', 'China', 1),
('Microsoft', 'USA', 1),
('Google', 'USA', 1),
('Amazon', 'USA', 1),
('Samsung', 'South Korea', 1),
('Nestle', 'Switzerland', 1),
('Nike', 'USA', 1),
('Adidas', 'Germany', 1),
('Puma', 'Germany', 1),
('Reebok', 'USA', 1),
('Under Armour', 'USA', 1),
('New Balance', 'USA', 1),
('Canon', 'Japan', 1),
('Nikon', 'Japan', 1);


-- Inserare date in tabelul Produse
INSERT INTO Produse (categorie_id, producator_id, nume_produs, cod_de_bare, activ)
VALUES 
(1, 1, 'iPhone', '0000000001', 1),
(1, 2, 'PlayStation', '0000000002', 1),
(1, 3, 'OLED TV', '0000000003', 1),
(1, 4, 'XPS Laptop', '0000000004', 1),
(1, 5, 'Spectre Laptop', '0000000005', 1),
(1, 6, 'ZenBook', '0000000006', 1),
(1, 7, 'ThinkPad', '0000000007', 1),
(1, 8, 'Surface', '0000000008', 1),
(1, 9, 'Pixel Phone', '0000000009', 1),
(1, 10, 'Echo Dot', '0000000010', 1),
(1, 11, 'Galaxy S', '0000000011', 1),
(2, 12, 'KitKat', '0000000012', 1),
(3, 13, 'Air Max', '0000000013', 1),
(3, 14, 'Ultraboost', '0000000014', 1),
(3, 15, 'Suede Classic', '0000000015', 1),
(3, 16, 'Nano X', '0000000016', 1),
(3, 17, 'Hovr', '0000000017', 1),
(3, 18, 'Fresh Foam', '0000000018', 1),
(1, 19, 'EOS Camera', '0000000019', 1),
(1, 20, 'D5600 Camera', '0000000020', 1);

-- Inserare date in tabelul Stocuri
INSERT INTO Stocuri (produs_id, cantitate, data_aprovizionare, data_expirare, pret_achizitie, pret_vanzare, activ)
VALUES 
(1, 49, '2024-01-01', '2025-01-01', 700, 1000.3, 1),
(2, 30, '2024-02-01', '2025-02-01', 300, 500, 1),
(3, 20, '2024-03-01', '2025-03-01', 900, 1300, 1),
(4, 10, '2024-04-01', '2025-04-01', 1000, 1500, 1),
(5, 25, '2024-05-01', '2025-05-01', 1100, 1600, 1),
(6, 35, '2024-06-01', '2025-06-01', 1200, 1700, 1),
(7, 40, '2024-07-01', '2025-07-01', 1300, 1800, 1),
(8, 50, '2024-08-01', '2025-08-01', 1400, 1900, 1),
(9, 60, '2024-09-01', '2025-09-01', 1500, 2000, 1),
(10, 70, '2024-10-01', '2025-10-01', 1600, 2100, 1),
(11, 80, '2024-11-01', '2025-11-01', 1700, 2200, 1),
(12, 90, '2024-12-01', '2025-12-01', 1.5, 2.5, 1),
(13, 100, '2025-01-01', '2026-01-01', 100, 150, 1),
(14, 110, '2025-02-01', '2026-02-01', 120, 180, 1),
(15, 120, '2025-03-01', '2026-03-01', 130, 190, 1),
(16, 130, '2025-04-01', '2026-04-01', 140, 200, 1),
(17, 140, '2025-05-01', '2026-05-01', 150, 210, 1),
(18, 150, '2025-06-01', '2026-06-01', 160, 220, 1),
(19, 160, '2025-07-01', '2026-07-01', 1700, 2300, 1),
(20, 170, '2025-08-01', '2026-08-01', 1800, 2400, 1);

-- Inserare date in tabelul Bonuri
INSERT INTO Bonuri (utilizator_id, numar_bon, data_eliberare, suma_totala, activ)
VALUES 
(1, 1001, '2024-01-01', 1500, 1),
(2, 1002, '2024-02-01', 2000, 1),
(3, 1003, '2024-03-01', 2500, 1),
(4, 1004, '2024-04-01', 3000, 1),
(5, 1005, '2024-05-01', 3500, 1),
(6, 1006, '2024-06-01', 4000, 1),
(7, 1007, '2024-07-01', 4500, 1),
(8, 1008, '2024-08-01', 5000, 1),
(9, 1009, '2024-09-01', 5500, 1),
(10, 1010, '2024-10-01', 6000, 1),
(11, 1011, '2024-11-01', 6500, 1),
(12, 1012, '2024-12-01', 7000, 1),
(13, 1013, '2025-01-01', 7500, 1),
(14, 1014, '2025-02-01', 8000, 1),
(15, 1015, '2025-03-01', 8500, 1),
(16, 1016, '2025-04-01', 9000, 1),
(17, 1017, '2025-05-01', 9500, 1),
(18, 1018, '2025-06-01', 10000, 1),
(19, 1019, '2025-07-01', 10500, 1),
(20, 1020, '2025-08-01', 11000, 1);

-- Inserare date in tabelul BonProdus

INSERT INTO BonProdus (bon_id, produs_id, cantitate, subtotal)
VALUES 
(1, 1, 1, 1000),
(2, 2, 2, 1000),
(3, 3, 1, 1300),
(4, 4, 1, 1500),
(5, 5, 1, 1600),
(6, 6, 1, 1700),
(7, 7, 1, 1800),
(8, 8, 1, 1900),
(9, 9, 1, 2000),
(10, 10, 1, 2100),
(11, 11, 1, 2200),
(12, 12, 2, 5),
(13, 13, 1, 150),
(14, 14, 1, 180),
(15, 15, 1, 190),
(16, 16, 1, 200),
(17, 17, 1, 210),
(18, 18, 1, 220),
(19, 19, 1, 2300),
(20, 20, 1, 2400),
(1, 2, 3, 1500),
(2, 3, 2, 2600),
(3, 4, 1, 1500),
(4, 5, 1, 1600),
(5, 6, 2, 3400),
(6, 7, 2, 3600),
(7, 8, 2, 3800),
(8, 9, 1, 2000),
(9, 10, 1, 2100),
(10, 11, 1, 2200),
(11, 12, 2, 5),
(12, 13, 1, 150),
(13, 14, 1, 180),
(14, 15, 1, 190),
(15, 16, 1, 200),
(16, 17, 1, 210),
(17, 18, 1, 220),
(18, 19, 1, 2300),
(19, 20, 1, 2400),
(20, 1, 2, 2000),
(1, 3, 2, 2600),
(2, 4, 1, 1500),
(3, 5, 1, 1600),
(4, 6, 2, 3400),
(5, 7, 2, 3600),
(6, 8, 2, 3800),
(7, 9, 1, 2000),
(8, 10, 1, 2100),
(9, 11, 1, 2200),
(10, 12, 2, 5),
(11, 13, 1, 150),
(12, 14, 1, 180),
(13, 15, 1, 190),
(14, 16, 1, 200),
(15, 17, 1, 210),
(16, 18, 1, 220),
(17, 19, 1, 2300),
(18, 20, 1, 2400),
(19, 1, 2, 2000),
(20, 2, 3, 3000),
(10, 9, 2, 2600),
(10, 18, 1, 1500),
(10, 19, 1, 1600),
(5, 16, 2, 3400),
(6, 14, 2, 3600),
(7, 15, 2, 3800),
(8, 16, 1, 2000),
(9, 17, 1, 2100),
(13, 18, 1, 2200),
(15, 19, 2, 5),
(16, 20, 1, 150),
(15, 13, 1, 180),
(18, 14, 1, 190),
(2, 15, 1, 200),
(6, 16, 1, 210),
(7, 17, 1, 220),
(20, 18, 1, 2300),
(6, 19, 1, 2400),
(16, 2, 2, 2000),
(5, 3, 3, 3000);
