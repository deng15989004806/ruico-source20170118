IF schema_id('base') IS NULL
    EXECUTE('CREATE SCHEMA [base]')
CREATE TABLE [base].[Category] (
    [Id] [uniqueidentifier] NOT NULL,
    [Sn] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](50),
    [SortOrder] [int] NOT NULL,
    [Depth] [int] NOT NULL,
    [ChildSnRulePrefix] [nvarchar](10),
    [ChildSnRuleNumberLength] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [CreatorId] [uniqueidentifier] NOT NULL,
    [Parent_Id] [uniqueidentifier],
    CONSTRAINT [PK_base.Category] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Sn] ON [base].[Category]([Sn])
CREATE INDEX [IX_Parent_Id] ON [base].[Category]([Parent_Id])
ALTER TABLE [base].[Category] ADD CONSTRAINT [FK_base.Category_base.Category_Parent_Id] FOREIGN KEY ([Parent_Id]) REFERENCES [base].[Category] ([Id])