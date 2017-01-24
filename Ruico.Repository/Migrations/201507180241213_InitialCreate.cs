namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "auth.Menu",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Code = c.String(maxLength: 150),
                        Url = c.String(maxLength: 150),
                        SortOrder = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Module_Id = c.Guid(nullable: false),
                        Parent_Menu_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("auth.Module", t => t.Module_Id, cascadeDelete: true)
                .ForeignKey("auth.Menu", t => t.Parent_Menu_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.Parent_Menu_Id);
            
            CreateTable(
                "auth.Module",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        SortOrder = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "auth.Permission",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Code = c.String(maxLength: 150),
                        ActionUrl = c.String(maxLength: 150),
                        SortOrder = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Menu_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("auth.Menu", t => t.Menu_Id, cascadeDelete: true)
                .Index(t => t.Menu_Id);
            
            CreateTable(
                "auth.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 150),
                        SortOrder = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        RoleGroup_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("auth.RoleGroup", t => t.RoleGroup_Id, cascadeDelete: true)
                .Index(t => t.RoleGroup_Id);
            
            CreateTable(
                "auth.RoleGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 150),
                        SortOrder = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "auth.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        LoginName = c.String(maxLength: 100),
                        LoginPwd = c.String(maxLength: 50),
                        Email = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        LastLoginToken = c.String(maxLength: 50),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "auth.Role_Permission",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        Permission_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Permission_Id })
                .ForeignKey("auth.Role", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("auth.Permission", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "auth.RoleGroup_User",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        RoleGroup_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.RoleGroup_Id })
                .ForeignKey("auth.User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("auth.RoleGroup", t => t.RoleGroup_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.RoleGroup_Id);
            
            CreateTable(
                "auth.User_Permission",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        Permission_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Permission_Id })
                .ForeignKey("auth.User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("auth.Permission", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Permission_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("auth.Permission", "Menu_Id", "auth.Menu");
            DropForeignKey("auth.Role", "RoleGroup_Id", "auth.RoleGroup");
            DropForeignKey("auth.User_Permission", "Permission_Id", "auth.Permission");
            DropForeignKey("auth.User_Permission", "User_Id", "auth.User");
            DropForeignKey("auth.RoleGroup_User", "RoleGroup_Id", "auth.RoleGroup");
            DropForeignKey("auth.RoleGroup_User", "User_Id", "auth.User");
            DropForeignKey("auth.Role_Permission", "Permission_Id", "auth.Permission");
            DropForeignKey("auth.Role_Permission", "Role_Id", "auth.Role");
            DropForeignKey("auth.Menu", "Parent_Menu_Id", "auth.Menu");
            DropForeignKey("auth.Menu", "Module_Id", "auth.Module");
            DropIndex("auth.User_Permission", new[] { "Permission_Id" });
            DropIndex("auth.User_Permission", new[] { "User_Id" });
            DropIndex("auth.RoleGroup_User", new[] { "RoleGroup_Id" });
            DropIndex("auth.RoleGroup_User", new[] { "User_Id" });
            DropIndex("auth.Role_Permission", new[] { "Permission_Id" });
            DropIndex("auth.Role_Permission", new[] { "Role_Id" });
            DropIndex("auth.Role", new[] { "RoleGroup_Id" });
            DropIndex("auth.Permission", new[] { "Menu_Id" });
            DropIndex("auth.Menu", new[] { "Parent_Menu_Id" });
            DropIndex("auth.Menu", new[] { "Module_Id" });
            DropTable("auth.User_Permission");
            DropTable("auth.RoleGroup_User");
            DropTable("auth.Role_Permission");
            DropTable("auth.User");
            DropTable("auth.RoleGroup");
            DropTable("auth.Role");
            DropTable("auth.Permission");
            DropTable("auth.Module");
            DropTable("auth.Menu");
        }
    }
}
