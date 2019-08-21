namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticeData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notice", "SenderType", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.Notice", "ReceiveId", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.Notice", "NoticeType", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.Notice", "ApplicationId", c => c.String(maxLength: 256));
            AddColumn("dbo.Notice", "ApplicationName", c => c.String(maxLength: 256));
            AlterColumn("dbo.ExpertInfo", "EducationHistor", c => c.String(nullable: false));
            AlterColumn("dbo.ExpertInfo", "WorkingHistory", c => c.String(nullable: false));
            AlterColumn("dbo.Notice", "Content", c => c.String(nullable: false, maxLength: 512));
            DropColumn("dbo.Notice", "Receiver");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notice", "Receiver", c => c.Int(nullable: false));
            AlterColumn("dbo.Notice", "Content", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.ExpertInfo", "WorkingHistory", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.ExpertInfo", "EducationHistor", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.Notice", "ApplicationName");
            DropColumn("dbo.Notice", "ApplicationId");
            DropColumn("dbo.Notice", "NoticeType");
            DropColumn("dbo.Notice", "ReceiveId");
            DropColumn("dbo.Notice", "SenderType");
        }
    }
}
