namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OperateRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "base.OperateRecordArchive",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sn = c.String(nullable: false, maxLength: 36),
                        UserId = c.Guid(),
                        OperatorName = c.String(maxLength: 20),
                        Operation = c.String(maxLength: 20),
                        Message = c.String(maxLength: 150),
                        OperateTime = c.DateTime(nullable: false),
                        Ip = c.String(maxLength: 20),
                        ExtendId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Sn);
            
            CreateTable(
                "base.OperateRecordExtend",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LongMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "base.OperateRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sn = c.String(nullable: false, maxLength: 36),
                        UserId = c.Guid(),
                        OperatorName = c.String(maxLength: 20),
                        Operation = c.String(maxLength: 20),
                        Message = c.String(maxLength: 150),
                        OperateTime = c.DateTime(nullable: false),
                        Ip = c.String(maxLength: 20),
                        ExtendId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Sn);
            
        }
        
        public override void Down()
        {
            DropIndex("base.OperateRecord", new[] { "Sn" });
            DropIndex("base.OperateRecordArchive", new[] { "Sn" });
            DropTable("base.OperateRecord");
            DropTable("base.OperateRecordExtend");
            DropTable("base.OperateRecordArchive");
        }
    }
}
