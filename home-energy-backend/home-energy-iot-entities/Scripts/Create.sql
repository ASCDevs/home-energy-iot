IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HomeEnergyIOT')
BEGIN
	CREATE DATABASE HomeEnergyIOT;
END;

GO
USE HomeEnergyIOT;
GO

--------------------Create Tables--------------------

CREATE TABLE [User](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	[Username] VARCHAR(255) NOT NULL,
	[Password] VARCHAR(255) NOT NULL,
	[SaltPassword] VARCHAR(255) NOT NULL,
	[CPF] CHAR(14) NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
	[RegisterDate] DATETIME NOT NULL
);

CREATE TABLE [House](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	[TypeAddress] VARCHAR(255) NOT NULL,
	[NameAddress] VARCHAR(255) NOT NULL,
	[NumberAddress] INT NOT NULL,
	[RegisterDate] DATETIME NOT NULL,
	[PeriodDaysReport] INT NOT NULL,

	[IdUser] INT,

	FOREIGN KEY ([IdUser]) REFERENCES [User]([Id])
);

CREATE TABLE [Device](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[IdentificationCode] VARCHAR(255) NOT NULL UNIQUE,
	[Name] VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[RegisterDate] DATETIME NOT NULL,
	[Watts] DECIMAL(5,2) NOT NULL,

	[IdHouse] INT,

	FOREIGN KEY ([IdHouse]) REFERENCES [House]([Id])
);

CREATE TABLE [HouseBill](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[MonthBill] DECIMAL(5,2) NOT NULL,
	[YearBill] DECIMAL(5,2) NOT NULL,
	[TariffBill] DECIMAL(5,2) NOT NULL,
	[ValuePerKWH] DECIMAL(5,2) NOT NULL,
	[BaseKWH] DECIMAL(5,2) NOT NULL,

	[IdHouse] INT,

	FOREIGN KEY ([IdHouse]) REFERENCES [House]([Id])
);

CREATE TABLE [DeviceReport](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[IdentificationCode] VARCHAR(255),
	[ReportDate] DATETIME NOT NULL,
	[WattsUsage] DECIMAL(5,2) NOT NULL,

	FOREIGN KEY ([IdentificationCode]) REFERENCES [Device]([IdentificationCode])
);

--------------------Default Inserts--------------------
-- Senha do usu�rio admin: admin
INSERT INTO [User] VALUES ('admin', 'admin', 'YWFTaY0XBRNdOIHfgz/yWsu5vnyZgSQqgAUeH7lSoHw=', 'm94DPZqHhs3/U8ccJ/oiosgDm/U=', '000.000.000-00', 'admin@admin.com', '2022-07-07 01:12:37.530')

INSERT INTO [House] VALUES('Minha Casa', 'Rua', 'Nome Rua', 100, '2022-08-29 12:00:00.000', 1, 1)

INSERT INTO [Device] VALUES('HU:34:DS4:D1', 'Computador', 'Computador da sala', '2022-08-29 12:00:00.000', 500, 1)
