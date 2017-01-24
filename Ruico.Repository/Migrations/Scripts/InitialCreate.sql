IF schema_id('auth') IS NULL
    EXECUTE('CREATE SCHEMA [auth]')
CREATE TABLE [auth].[Menu] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [Code] [nvarchar](150),
    [Url] [nvarchar](150),
    [SortOrder] [int] NOT NULL,
    [Depth] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [Module_Id] [uniqueidentifier] NOT NULL,
    [Parent_Menu_Id] [uniqueidentifier],
    CONSTRAINT [PK_auth.Menu] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Module_Id] ON [auth].[Menu]([Module_Id])
CREATE INDEX [IX_Parent_Menu_Id] ON [auth].[Menu]([Parent_Menu_Id])
CREATE TABLE [auth].[Module] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [SortOrder] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    CONSTRAINT [PK_auth.Module] PRIMARY KEY ([Id])
)
CREATE TABLE [auth].[Permission] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [Code] [nvarchar](150),
    [ActionUrl] [nvarchar](150),
    [SortOrder] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [Menu_Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_auth.Permission] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Menu_Id] ON [auth].[Permission]([Menu_Id])
CREATE TABLE [auth].[Role] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [Description] [nvarchar](150),
    [SortOrder] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    [RoleGroup_Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_auth.Role] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_RoleGroup_Id] ON [auth].[Role]([RoleGroup_Id])
CREATE TABLE [auth].[RoleGroup] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [Description] [nvarchar](150),
    [SortOrder] [int] NOT NULL,
    [Created] [datetime] NOT NULL,
    CONSTRAINT [PK_auth.RoleGroup] PRIMARY KEY ([Id])
)
CREATE TABLE [auth].[User] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50),
    [LoginName] [nvarchar](100),
    [LoginPwd] [nvarchar](50),
    [Email] [nvarchar](100),
    [Created] [datetime] NOT NULL,
    [LastLoginToken] [nvarchar](50),
    [LastLogin] [datetime] NOT NULL,
    CONSTRAINT [PK_auth.User] PRIMARY KEY ([Id])
)
CREATE TABLE [auth].[Role_Permission] (
    [Role_Id] [uniqueidentifier] NOT NULL,
    [Permission_Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_auth.Role_Permission] PRIMARY KEY ([Role_Id], [Permission_Id])
)
CREATE INDEX [IX_Role_Id] ON [auth].[Role_Permission]([Role_Id])
CREATE INDEX [IX_Permission_Id] ON [auth].[Role_Permission]([Permission_Id])
CREATE TABLE [auth].[RoleGroup_User] (
    [User_Id] [uniqueidentifier] NOT NULL,
    [RoleGroup_Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_auth.RoleGroup_User] PRIMARY KEY ([User_Id], [RoleGroup_Id])
)
CREATE INDEX [IX_User_Id] ON [auth].[RoleGroup_User]([User_Id])
CREATE INDEX [IX_RoleGroup_Id] ON [auth].[RoleGroup_User]([RoleGroup_Id])
CREATE TABLE [auth].[User_Permission] (
    [User_Id] [uniqueidentifier] NOT NULL,
    [Permission_Id] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_auth.User_Permission] PRIMARY KEY ([User_Id], [Permission_Id])
)
CREATE INDEX [IX_User_Id] ON [auth].[User_Permission]([User_Id])
CREATE INDEX [IX_Permission_Id] ON [auth].[User_Permission]([Permission_Id])
ALTER TABLE [auth].[Menu] ADD CONSTRAINT [FK_auth.Menu_auth.Module_Module_Id] FOREIGN KEY ([Module_Id]) REFERENCES [auth].[Module] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[Menu] ADD CONSTRAINT [FK_auth.Menu_auth.Menu_Parent_Menu_Id] FOREIGN KEY ([Parent_Menu_Id]) REFERENCES [auth].[Menu] ([Id])
ALTER TABLE [auth].[Permission] ADD CONSTRAINT [FK_auth.Permission_auth.Menu_Menu_Id] FOREIGN KEY ([Menu_Id]) REFERENCES [auth].[Menu] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[Role] ADD CONSTRAINT [FK_auth.Role_auth.RoleGroup_RoleGroup_Id] FOREIGN KEY ([RoleGroup_Id]) REFERENCES [auth].[RoleGroup] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[Role_Permission] ADD CONSTRAINT [FK_auth.Role_Permission_auth.Role_Role_Id] FOREIGN KEY ([Role_Id]) REFERENCES [auth].[Role] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[Role_Permission] ADD CONSTRAINT [FK_auth.Role_Permission_auth.Permission_Permission_Id] FOREIGN KEY ([Permission_Id]) REFERENCES [auth].[Permission] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[RoleGroup_User] ADD CONSTRAINT [FK_auth.RoleGroup_User_auth.User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [auth].[User] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[RoleGroup_User] ADD CONSTRAINT [FK_auth.RoleGroup_User_auth.RoleGroup_RoleGroup_Id] FOREIGN KEY ([RoleGroup_Id]) REFERENCES [auth].[RoleGroup] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[User_Permission] ADD CONSTRAINT [FK_auth.User_Permission_auth.User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [auth].[User] ([Id]) ON DELETE CASCADE
ALTER TABLE [auth].[User_Permission] ADD CONSTRAINT [FK_auth.User_Permission_auth.Permission_Permission_Id] FOREIGN KEY ([Permission_Id]) REFERENCES [auth].[Permission] ([Id]) ON DELETE CASCADE