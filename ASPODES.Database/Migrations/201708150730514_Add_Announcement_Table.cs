namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Announcement_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcement",
                c => new
                    {
                        AnnouncementId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        PublisherId = c.String(),
                        Type = c.String(maxLength: 4),
                        Status = c.String(maxLength: 4),
                        Publisher_UserId = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.AnnouncementId)
                .ForeignKey("dbo.User", t => t.Publisher_UserId)
                .Index(t => t.Publisher_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Announcement", "Publisher_UserId", "dbo.User");
            DropIndex("dbo.Announcement", new[] { "Publisher_UserId" });
            DropTable("dbo.Announcement");
        }
    }
}
