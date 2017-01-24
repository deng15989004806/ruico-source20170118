namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cargo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "base.Cargo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sn = c.String(nullable: false, maxLength: 32),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.Guid(nullable: false),
                        Category_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("base.Category", t => t.Category_Id)
                .Index(t => t.Sn)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("base.Cargo", "Category_Id", "base.Category");
            DropIndex("base.Cargo", new[] { "Category_Id" });
            DropIndex("base.Cargo", new[] { "Sn" });
            DropTable("base.Cargo");
        }
    }
}
