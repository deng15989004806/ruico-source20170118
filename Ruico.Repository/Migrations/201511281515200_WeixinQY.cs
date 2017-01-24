namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WeixinQY : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "weixin.AppMenu",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Key = c.String(maxLength: 150),
                        Url = c.String(maxLength: 150),
                        SortOrder = c.Int(nullable: false),
                        AppId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Parent_Menu_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("weixin.AppMenu", t => t.Parent_Menu_Id)
                .Index(t => t.Parent_Menu_Id);
            
            CreateTable(
                "hr.Department",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        DepartmentId = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        ParentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "hr.Member",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Userid = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        WeixinId = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        Gender = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 50),
                        Email = c.String(maxLength: 150),
                        Status = c.Int(nullable: false),
                        Avatar = c.String(maxLength: 300),
                        Enable = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "hr.Department_Member",
                c => new
                    {
                        Member_Id = c.Guid(nullable: false),
                        Department_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_Id, t.Department_Id })
                .ForeignKey("hr.Member", t => t.Member_Id, cascadeDelete: true)
                .ForeignKey("hr.Department", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Member_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("hr.Department_Member", "Department_Id", "hr.Department");
            DropForeignKey("hr.Department_Member", "Member_Id", "hr.Member");
            DropForeignKey("weixin.AppMenu", "Parent_Menu_Id", "weixin.AppMenu");
            DropIndex("hr.Department_Member", new[] { "Department_Id" });
            DropIndex("hr.Department_Member", new[] { "Member_Id" });
            DropIndex("weixin.AppMenu", new[] { "Parent_Menu_Id" });
            DropTable("hr.Department_Member");
            DropTable("hr.Member");
            DropTable("hr.Department");
            DropTable("weixin.AppMenu");
        }
    }
}
