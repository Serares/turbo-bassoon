### Logs for creating and querying a TPH strategy
```bash
Database deleted: False
Database created: True
SQL script used to create the database:
CREATE TABLE [People] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(40) NOT NULL,
    [Discriminator] nvarchar(8) NOT NULL,
    [HireDate] datetime2 NULL,
    [Subject] nvarchar(max) NULL,
    CONSTRAINT [PK_People] PRIMARY KEY ([Id])
);
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'Name', N'Subject') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Discriminator], [Name], [Subject])
VALUES (1, N'Student', N'John', N'Math'),
(2, N'Student', N'Jane', N'Science');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'Name', N'Subject') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'HireDate', N'Name') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Discriminator], [HireDate], [Name])
VALUES (3, N'Employee', '2020-01-01T00:00:00.0000000', N'Jim'),
(4, N'Employee', '2024-01-26T00:00:00.0000000', N'Jill');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Discriminator', N'HireDate', N'Name') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;
GO



All people:
  1: John
    Student studying Math
  2: Jane
    Student studying Science
  3: Jim
    Employee hired on 2020-01-01
  4: Jill
    Employee hired on 2024-01-26

Students only:
  1: John studies Math
  2: Jane studies Science

Employees only:
  3: Jim hired on 2020-01-01
  4: Jill hired on 2024-01-26
```

### Using TPT mapping strategy
```bash
Database deleted: True
Database created: True
SQL script used to create the database:
CREATE TABLE [People] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_People] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Employees] (
    [Id] int NOT NULL,
    [HireDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_People_Id] FOREIGN KEY ([Id]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [Students] (
    [Id] int NOT NULL,
    [Subject] nvarchar(max) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Students_People_Id] FOREIGN KEY ([Id]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Name])
VALUES (1, N'John'),
(2, N'Jane'),
(3, N'Jim'),
(4, N'Jill');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'HireDate') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] ON;
INSERT INTO [Employees] ([Id], [HireDate])
VALUES (3, '2020-01-01T00:00:00.0000000'),
(4, '2024-01-26T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'HireDate') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Subject') AND [object_id] = OBJECT_ID(N'[Students]'))
    SET IDENTITY_INSERT [Students] ON;
INSERT INTO [Students] ([Id], [Subject])
VALUES (1, N'Math'),
(2, N'Science');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Subject') AND [object_id] = OBJECT_ID(N'[Students]'))
    SET IDENTITY_INSERT [Students] OFF;
GO



All people:
  1: John
    Student studying Math
  2: Jane
    Student studying Science
  3: Jim
    Employee hired on 2020-01-01
  4: Jill
    Employee hired on 2024-01-26

Students only:
  1: John studies Math
  2: Jane studies Science

Employees only:
  3: Jim hired on 2020-01-01
  4: Jill hired on 2024-01-26
```

### TPC mapping strategy
```bash
SQL script used to create the database:
CREATE SEQUENCE [PersonIds] AS int START WITH 5 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
GO


CREATE TABLE [Employees] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR [PersonIds]),
    [Name] nvarchar(40) NOT NULL,
    [HireDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Students] (
    [Id] int NOT NULL DEFAULT (NEXT VALUE FOR [PersonIds]),
    [Name] nvarchar(40) NOT NULL,
    [Subject] nvarchar(max) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id])
);
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'HireDate', N'Name') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] ON;
INSERT INTO [Employees] ([Id], [HireDate], [Name])
VALUES (3, '2020-01-01T00:00:00.0000000', N'Jim'),
(4, '2024-01-26T00:00:00.0000000', N'Jill');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'HireDate', N'Name') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Subject') AND [object_id] = OBJECT_ID(N'[Students]'))
    SET IDENTITY_INSERT [Students] ON;
INSERT INTO [Students] ([Id], [Name], [Subject])
VALUES (1, N'John', N'Math'),
(2, N'Jane', N'Science');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Subject') AND [object_id] = OBJECT_ID(N'[Students]'))
    SET IDENTITY_INSERT [Students] OFF;
GO



All people:
  1: John
    Student studying Math
  2: Jane
    Student studying Science
  3: Jim
    Employee hired on 2020-01-01
  4: Jill
    Employee hired on 2024-01-26

Students only:
  1: John studies Math
  2: Jane studies Science

Employees only:
  3: Jim hired on 2020-01-01
  4: Jill hired on 2024-01-26
```