namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_Ship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "base.Company",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sn = c.String(nullable: false, maxLength: 32),
                        Name = c.String(maxLength: 50),
                        Contact = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Address = c.String(maxLength: 250),
                        Postcode = c.String(maxLength: 50),
                        Email = c.String(maxLength: 150),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.Guid(nullable: false),
                        Category_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("base.Category", t => t.Category_Id)
                .Index(t => t.Sn)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "base.Ship",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sn = c.String(nullable: false, maxLength: 32),
                        Name = c.String(maxLength: 50),
                        FormerName = c.String(maxLength: 50),
                        Description = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Sn);
            
        }
        
        public override void Down()
        {
            DropForeignKey("base.Company", "Category_Id", "base.Category");
            DropIndex("base.Ship", new[] { "Sn" });
            DropIndex("base.Company", new[] { "Category_Id" });
            DropIndex("base.Company", new[] { "Sn" });
            DropTable("base.Ship");
            DropTable("base.Company");
        }
    }
}
