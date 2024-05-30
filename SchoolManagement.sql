CREATE SCHEMA [Security]
GO

CREATE SCHEMA [HumanResources]
GO

CREATE TABLE [Security].[User] (
  [Id] uniqueidentifier PRIMARY KEY NOT NULL,
  [Name] varchar(50) UNIQUE NOT NULL,
  [Password] varbinary(max) NOT NULL,
  [Enabled] bit NOT NULL DEFAULT (1),
  [RoleId] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [Security].[Role] (
  [Id] uniqueidentifier PRIMARY KEY NOT NULL,
  [Name] varchar(50) UNIQUE NOT NULL,
  [Enabled] bit NOT NULL DEFAULT (1)
)
GO

CREATE TABLE [HumanResources].[Person] (
  [Id] uniqueidentifier PRIMARY KEY NOT NULL,
  [Name] varchar(255) NOT NULL,
  [IdNumber] varchar(255) UNIQUE,
  [Birthdate] date NOT NULL,
  [Enabled] bit NOT NULL DEFAULT (1),
  [Address] nvarchar(255),
  [Mail] nvarchar(255) UNIQUE NOT NULL,
  [UserId] uniqueidentifier NOT NULL
)
GO

ALTER TABLE [Security].[User] ADD FOREIGN KEY ([RoleId]) REFERENCES [Security].[Role] ([Id])
GO

ALTER TABLE [HumanResources].[Person] ADD FOREIGN KEY ([UserId]) REFERENCES [Security].[User] ([Id])
GO
