namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "base.Category",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sn = c.String(nullable: false, maxLength: 32),
                        Name = c.String(maxLength: 50),
                        SortOrder = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        ChildSnRulePrefix = c.String(maxLength: 10),
                        ChildSnRuleNumberLength = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.Guid(nullable: false),
                        Parent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("base.Category", t => t.Parent_Id)
                .Index(t => t.Sn)
                .Index(t => t.Parent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("base.Category", "Parent_Id", "base.Category");
            DropIndex("base.Category", new[] { "Parent_Id" });
            DropIndex("base.Category", new[] { "Sn" });
            DropTable("base.Category");
        }
    }
}
