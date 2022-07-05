IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HomeEnergyIOT')
BEGIN
	CREATE DATABASE HomeEnergyIOT;
END;

GO
USE HomeEnergyIOT;
GO

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

CREATE TABLE [UserHouse](
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
	[Name] VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[RegisterDate] DATETIME NOT NULL,
	[Watts] DECIMAL(5,2) NOT NULL,

	[IdHouse] INT,

	FOREIGN KEY ([IdHouse]) REFERENCES [UserHouse]([Id])
);

CREATE TABLE [HouseBill](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[MonthBill] DECIMAL(5,2) NOT NULL,
	[YearBill] DECIMAL(5,2) NOT NULL,
	[TariffBill] DECIMAL(5,2) NOT NULL,
	[ValuePerKWH] DECIMAL(5,2) NOT NULL,
	[BaseKWH] DECIMAL(5,2) NOT NULL,

	[IdHouse] INT,

	FOREIGN KEY ([IdHouse]) REFERENCES [UserHouse]([Id])
);

CREATE TABLE [DeviceReport](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[StartDate] DATETIME NOT NULL,
	[LastDate] DATETIME NOT NULL,
	[Consumption] DECIMAL(5,2) NOT NULL,

	[IdDevice] INT,

	FOREIGN KEY ([IdDevice]) REFERENCES [Device]([Id])
);