namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SerialNumber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "system.SerialNumber",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Prefix = c.String(maxLength: 10),
                        DateNumber = c.String(maxLength: 8),
                        Sequence = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("system.SerialNumber");
        }
    }
}
