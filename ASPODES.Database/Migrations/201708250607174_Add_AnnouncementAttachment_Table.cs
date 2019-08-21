namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AnnouncementAttachment_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnouncementAttachment",
                c => new
                    {
                        AnnouncementAttachmentId = c.Int(nullable: false, identity: true),
                        AnnouncementId = c.Int(nullable: false),
                        Name = c.String(),
                        RelativeURL = c.String(),
                    })
                .PrimaryKey(t => t.AnnouncementAttachmentId)
                .ForeignKey("dbo.Announcement", t => t.AnnouncementId, cascadeDelete: true)
                .Index(t => t.AnnouncementId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnnouncementAttachment", "AnnouncementId", "dbo.Announcement");
            DropIndex("dbo.AnnouncementAttachment", new[] { "AnnouncementId" });
            DropTable("dbo.AnnouncementAttachment");
        }
    }
}
