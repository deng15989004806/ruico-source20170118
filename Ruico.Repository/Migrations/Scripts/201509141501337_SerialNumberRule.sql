IF schema_id('base') IS NULL
    EXECUTE('CREATE SCHEMA [base]')
CREATE TABLE [base].[SerialNumberRule] (
    [Id] [uniqueidentifier] NOT NULL,
    [RuleName] [nvarchar](20) NOT NULL,
    [Prefix] [nvarchar](10),
    [UseDateNumber] [bit] NOT NULL,
    [NumberLength] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [CreatorId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_base.SerialNumberRule] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_RuleName] ON [base].[SerialNumberRule]([RuleName])