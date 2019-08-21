namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Notice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notice", "NoticeType", c => c.Int(nullable: false));
            AddColumn("dbo.Notice", "URL", c => c.String(nullable: false, maxLength: 512));
            CreateIndex("dbo.Notice", "ReceiveId");
            AddForeignKey("dbo.Notice", "ReceiveId", "dbo.User", "UserId");
            DropColumn("dbo.Notice", "SenderType");
            DropColumn("dbo.Notice", "Type");
            DropColumn("dbo.Notice", "ApplicationId");
            DropColumn("dbo.Notice", "ApplicationName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notice", "ApplicationName", c => c.String(maxLength: 256));
            AddColumn("dbo.Notice", "ApplicationId", c => c.String(maxLength: 256));
            AddColumn("dbo.Notice", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Notice", "SenderType", c => c.String(nullable: false, maxLength: 256));
            DropForeignKey("dbo.Notice", "ReceiveId", "dbo.User");
            DropIndex("dbo.Notice", new[] { "ReceiveId" });
            DropColumn("dbo.Notice", "URL");
            DropColumn("dbo.Notice", "NoticeType");
        }
    }
}
