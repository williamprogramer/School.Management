CREATE SCHEMA [Security]
GO

CREATE TABLE [Security].[User] (
  [Id] uniqueidentifier PRIMARY KEY NOT NULL,
  [UserName] varchar(255) UNIQUE NOT NULL,
  [Password] varbinary(max) NOT NULL,
  [Enabled] bit NOT NULL DEFAULT (1),
  [FullName] varchar(255) NOT NULL,
  [IdNumber] varchar(255) UNIQUE,
  [Birthdate] date NOT NULL,
  [Address] nvarchar(255),
  [Mail] nvarchar(255) UNIQUE NOT NULL,
  [RoleId] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [Security].[Role] (
  [Id] uniqueidentifier PRIMARY KEY NOT NULL,
  [Name] varchar(50) UNIQUE NOT NULL,
  [Enabled] bit NOT NULL DEFAULT (1)
)
GO

ALTER TABLE [Security].[User] ADD FOREIGN KEY ([RoleId]) REFERENCES [Security].[Role] ([Id])
GO
