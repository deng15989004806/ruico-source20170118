IF schema_id('weixin') IS NULL
    EXECUTE('CREATE SCHEMA [weixin]')
CREATE TABLE [weixin].[AppMenu] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [Key] [nvarchar](150),
    [Url] [nvarchar](150),
    [SortOrder] [int] NOT NULL,
    [AppId] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [Parent_Menu_Id] [uniqueidentifier],
    CONSTRAINT [PK_weixin.AppMenu] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Parent_Menu_Id] ON [weixin].[AppMenu]([Parent_Menu_Id])
IF schema_id('hr') IS NULL
    EXECUTE('CREATE SCHEMA [hr]')
CREATE TABLE [hr].[Department] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [DepartmentId] [int] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [ParentId] [int] NOT NULL,
    CONSTRAINT [PK_hr.Department] PRIMARY KEY ([Id])
)
CREATE TABLE [hr].[Member] (
    [Id] [uniqueidentifier] NOT NULL,
    [Userid] [nvarchar](50),
    [Name] [nvarchar](50),
    [WeixinId] [nvarchar](50),
    [Position] [nvarchar](50),
    [Gender] [int] NOT NULL,
    [Mobile] [nvarchar](50),
    [Email] [nvarchar](150),
    [Status] [int] NOT NULL,
    [Avatar] [nvarchar](300),
    [Enable] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    CONSTRAINT [PK_hr.Member] PRIMARY KEY ([Id])
)
CREATE TABLE [hr].[Department_Member] (
    [Member_Id] [uniqueidentifier] NOT NULL,
    [Department_Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_hr.Department_Member] PRIMARY KEY ([Member_Id], [Department_Id])
)
CREATE INDEX [IX_Member_Id] ON [hr].[Department_Member]([Member_Id])
CREATE INDEX [IX_Department_Id] ON [hr].[Department_Member]([Department_Id])
ALTER TABLE [weixin].[AppMenu] ADD CONSTRAINT [FK_weixin.AppMenu_weixin.AppMenu_Parent_Menu_Id] FOREIGN KEY ([Parent_Menu_Id]) REFERENCES [weixin].[AppMenu] ([Id])
ALTER TABLE [hr].[Department_Member] ADD CONSTRAINT [FK_hr.Department_Member_hr.Member_Member_Id] FOREIGN KEY ([Member_Id]) REFERENCES [hr].[Member] ([Id]) ON DELETE CASCADE
ALTER TABLE [hr].[Department_Member] ADD CONSTRAINT [FK_hr.Department_Member_hr.Department_Department_Id] FOREIGN KEY ([Department_Id]) REFERENCES [hr].[Department] ([Id]) ON DELETE CASCADE