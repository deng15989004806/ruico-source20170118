IF schema_id('base') IS NULL
    EXECUTE('CREATE SCHEMA [base]')
CREATE TABLE [base].[OperateRecordArchive] (
    [Id] [uniqueidentifier] NOT NULL,
    [Sn] [nvarchar](36) NOT NULL,
    [UserId] [uniqueidentifier],
    [OperatorName] [nvarchar](20),
    [Operation] [nvarchar](20),
    [Message] [nvarchar](150),
    [OperateTime] [datetime] NOT NULL,
    [Ip] [nvarchar](20),
    [ExtendId] [uniqueidentifier],
    CONSTRAINT [PK_base.OperateRecordArchive] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Sn] ON [base].[OperateRecordArchive]([Sn])
CREATE TABLE [base].[OperateRecordExtend] (
    [Id] [uniqueidentifier] NOT NULL,
    [LongMessage] [nvarchar](max),
    CONSTRAINT [PK_base.OperateRecordExtend] PRIMARY KEY ([Id])
)
CREATE TABLE [base].[OperateRecord] (
    [Id] [uniqueidentifier] NOT NULL,
    [Sn] [nvarchar](36) NOT NULL,
    [UserId] [uniqueidentifier],
    [OperatorName] [nvarchar](20),
    [Operation] [nvarchar](20),
    [Message] [nvarchar](150),
    [OperateTime] [datetime] NOT NULL,
    [Ip] [nvarchar](20),
    [ExtendId] [uniqueidentifier],
    CONSTRAINT [PK_base.OperateRecord] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Sn] ON [base].[OperateRecord]([Sn])