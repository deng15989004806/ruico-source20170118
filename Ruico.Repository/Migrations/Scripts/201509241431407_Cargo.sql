IF schema_id('base') IS NULL
    EXECUTE('CREATE SCHEMA [base]')
CREATE TABLE [base].[Cargo] (
    [Id] [uniqueidentifier] NOT NULL,
    [Sn] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](50),
    [Description] [nvarchar](max),
    [Created] [datetime] NOT NULL,
    [CreatorId] [uniqueidentifier] NOT NULL,
    [Category_Id] [uniqueidentifier],
    CONSTRAINT [PK_base.Cargo] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Sn] ON [base].[Cargo]([Sn])
CREATE INDEX [IX_Category_Id] ON [base].[Cargo]([Category_Id])
ALTER TABLE [base].[Cargo] ADD CONSTRAINT [FK_base.Cargo_base.Category_Category_Id] FOREIGN KEY ([Category_Id]) REFERENCES [base].[Category] ([Id])