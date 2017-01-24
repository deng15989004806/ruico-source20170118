namespace Ruico.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KaoQin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "kaoqin.ChuChai",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        DepartmentId = c.Int(nullable: false),
                        Department = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        OutTime = c.DateTime(nullable: false),
                        InTime = c.DateTime(nullable: false),
                        OutPlace = c.String(maxLength: 50),
                        OutReason = c.String(maxLength: 250),
                        DepartmentOpinion = c.String(maxLength: 150),
                        DepartmentOpinionApproverId = c.String(maxLength: 50),
                        GeneralManagerOfficeOpinion = c.String(maxLength: 150),
                        GeneralManagerOfficeOpinionApproverId = c.String(maxLength: 50),
                        CompanyLeaderOpinion = c.String(maxLength: 150),
                        CompanyLeaderOpinionApproverId = c.String(maxLength: 150),
                        Status = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        Canceled = c.DateTime(nullable: false),
                        Approved = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "kaoqin.WaiQin",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        DepartmentId = c.Int(nullable: false),
                        Department = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        OutTime = c.DateTime(nullable: false),
                        InTime = c.DateTime(nullable: false),
                        OutPlace = c.String(maxLength: 50),
                        OutReason = c.String(maxLength: 250),
                        DepartmentOpinion = c.String(maxLength: 150),
                        DepartmentOpinionApproverId = c.String(maxLength: 50),
                        GeneralManagerOfficeOpinion = c.String(maxLength: 150),
                        GeneralManagerOfficeOpinionApproverId = c.String(maxLength: 50),
                        CompanyLeaderOpinion = c.String(maxLength: 150),
                        CompanyLeaderOpinionApproverId = c.String(maxLength: 150),
                        Status = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        Canceled = c.DateTime(nullable: false),
                        Approved = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "kaoqin.WeiDaKa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        DepartmentId = c.Int(nullable: false),
                        Department = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        InputTime = c.DateTime(nullable: false),
                        ActionTime = c.DateTime(nullable: false),
                        Type = c.String(maxLength: 50),
                        Reason = c.String(maxLength: 250),
                        DepartmentOrCompanyOpinion = c.String(maxLength: 150),
                        DepartmentOrCompanyOpinionApproverId = c.String(maxLength: 50),
                        Status = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        Canceled = c.DateTime(nullable: false),
                        Approved = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "kaoqin.XiuJia",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        DepartmentId = c.Int(nullable: false),
                        Department = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        InputTime = c.DateTime(nullable: false),
                        ActionStartTime = c.DateTime(nullable: false),
                        ActionEndTime = c.DateTime(nullable: false),
                        ActionDays = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActionHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.String(maxLength: 50),
                        Reason = c.String(maxLength: 250),
                        DepartmentSupervisorOpinion = c.String(maxLength: 150),
                        DepartmentSupervisorOpinionApproverId = c.String(maxLength: 50),
                        DepartmentManagerOpinion = c.String(maxLength: 150),
                        DepartmentManagerOpinionApproverId = c.String(maxLength: 50),
                        CompanyLeaderOpinion = c.String(maxLength: 150),
                        CompanyLeaderOpinionApproverId = c.String(maxLength: 150),
                        Status = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        Canceled = c.DateTime(nullable: false),
                        Approved = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("kaoqin.XiuJia");
            DropTable("kaoqin.WeiDaKa");
            DropTable("kaoqin.WaiQin");
            DropTable("kaoqin.ChuChai");
        }
    }
}
