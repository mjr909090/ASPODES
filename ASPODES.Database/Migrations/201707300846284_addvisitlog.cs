namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvisitlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VisitLog",
                c => new
                    {
                        VisitId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 256),
                        ControllerName = c.String(maxLength: 512),
                        ActionName = c.String(maxLength: 512),
                        ActionNameDetail = c.String(maxLength: 512),
                        BeginTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        CostTime = c.Double(),
                        UserHostName = c.String(),
                        UrlReferrer = c.String(),
                        RequestParamaters = c.String(),
                        ResponseResult = c.String(),
                        UsersAgents = c.String(),
                        IPAddress = c.String(maxLength: 128),
                        UserDevice = c.String(maxLength: 2048),
                        UserPartform = c.String(maxLength: 256),
                        UserBrowser = c.String(maxLength: 2048),
                        UserOS = c.String(maxLength: 2048),
                    })
                .PrimaryKey(t => t.VisitId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VisitLog");
        }
    }
}
