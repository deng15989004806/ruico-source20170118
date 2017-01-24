IF schema_id('base') IS NULL
    EXECUTE('CREATE SCHEMA [base]')
CREATE TABLE [base].[Company] (
    [Id] [uniqueidentifier] NOT NULL,
    [Sn] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](50),
    [Contact] [nvarchar](50),
    [Phone] [nvarchar](50),
    [Fax] [nvarchar](50),
    [Mobile] [nvarchar](50),
    [Address] [nvarchar](250),
    [Postcode] [nvarchar](50),
    [Email] [nvarchar](150),
    [Created] [datetime] NOT NULL,
    [CreatorId] [uniqueidentifier] NOT NULL,
    [Category_Id] [uniqueidentifier],
    CONSTRAINT [PK_base.Company] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Sn] ON [base].[Company]([Sn])
CREATE INDEX [IX_Category_Id] ON [base].[Company]([Category_Id])
CREATE TABLE [base].[Ship] (
    [Id] [uniqueidentifier] NOT NULL,
    [Sn] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](50),
    [FormerName] [nvarchar](50),
    [Description] [nvarchar](max),
    [Created] [datetime] NOT NULL,
    [CreatorId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_base.Ship] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Sn] ON [base].[Ship]([Sn])
ALTER TABLE [base].[Company] ADD CONSTRAINT [FK_base.Company_base.Category_Category_Id] FOREIGN KEY ([Category_Id]) REFERENCES [base].[Category] ([Id])