namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTablesyssetinghistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SysSettingHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationSubmitBeginTime = c.DateTime(),
                        ApplicationSubmitDeadline = c.DateTime(),
                        ApplicationVerifyDeadline = c.DateTime(),
                        ApplicationExpertDeadline = c.DateTime(),
                        ApplicationStartYear = c.Int(),
                        UpdateTime = c.DateTime(),
                        UpdateIp = c.String(maxLength: 40),
                        UpdateId = c.String(maxLength: 256),
                        Remark = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SysSettingHistory");
        }
    }
}
