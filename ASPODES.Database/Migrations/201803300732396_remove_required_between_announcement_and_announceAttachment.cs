namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_required_between_announcement_and_announceAttachment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnnouncementAttachment", "AnnouncementId", "dbo.Announcement");
            DropIndex("dbo.AnnouncementAttachment", new[] { "AnnouncementId" });
            AlterColumn("dbo.AnnouncementAttachment", "AnnouncementId", c => c.Int());
            CreateIndex("dbo.AnnouncementAttachment", "AnnouncementId");
            AddForeignKey("dbo.AnnouncementAttachment", "AnnouncementId", "dbo.Announcement", "AnnouncementId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnnouncementAttachment", "AnnouncementId", "dbo.Announcement");
            DropIndex("dbo.AnnouncementAttachment", new[] { "AnnouncementId" });
            AlterColumn("dbo.AnnouncementAttachment", "AnnouncementId", c => c.Int(nullable: false));
            CreateIndex("dbo.AnnouncementAttachment", "AnnouncementId");
            AddForeignKey("dbo.AnnouncementAttachment", "AnnouncementId", "dbo.Announcement", "AnnouncementId", cascadeDelete: true);
        }
    }
}
