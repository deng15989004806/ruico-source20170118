namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SerialNumberRule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "base.SerialNumberRule",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RuleName = c.String(nullable: false, maxLength: 20),
                        Prefix = c.String(maxLength: 10),
                        UseDateNumber = c.Boolean(nullable: false),
                        NumberLength = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RuleName);
            
        }
        
        public override void Down()
        {
            DropIndex("base.SerialNumberRule", new[] { "RuleName" });
            DropTable("base.SerialNumberRule");
        }
    }
}
