namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNoticeAndNoticeTemplateTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NoticeTemplates", "NoticeType", c => c.Int(nullable: false));
            AddColumn("dbo.NoticeTemplates", "URL", c => c.String(maxLength: 512));
            DropColumn("dbo.Notice", "NoticeType");
            DropColumn("dbo.Notice", "URL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notice", "URL", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.Notice", "NoticeType", c => c.Int(nullable: false));
            DropColumn("dbo.NoticeTemplates", "URL");
            DropColumn("dbo.NoticeTemplates", "NoticeType");
        }
    }
}
