namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseToSystem : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "base.OperateRecordArchive", newSchema: "system");
            MoveTable(name: "base.OperateRecordExtend", newSchema: "system");
            MoveTable(name: "base.OperateRecord", newSchema: "system");
        }
        
        public override void Down()
        {
            MoveTable(name: "system.OperateRecord", newSchema: "base");
            MoveTable(name: "system.OperateRecordExtend", newSchema: "base");
            MoveTable(name: "system.OperateRecordArchive", newSchema: "base");
        }
    }
}
