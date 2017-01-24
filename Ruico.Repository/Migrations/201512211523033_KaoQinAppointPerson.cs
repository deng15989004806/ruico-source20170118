namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KaoQinAppointPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("kaoqin.ChuChai", "AppointPerson", c => c.String(maxLength: 50));
            AddColumn("kaoqin.WaiQin", "AppointPerson", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("kaoqin.WaiQin", "AppointPerson");
            DropColumn("kaoqin.ChuChai", "AppointPerson");
        }
    }
}
