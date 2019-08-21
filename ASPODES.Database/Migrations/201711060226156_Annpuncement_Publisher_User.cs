namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Annpuncement_Publisher_User : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Announcement", new[] { "Publisher_UserId" });
            DropColumn("dbo.Announcement", "PublisherId");
            RenameColumn(table: "dbo.Announcement", name: "Publisher_UserId", newName: "PublisherId");
            AlterColumn("dbo.Announcement", "PublisherId", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Announcement", "PublisherId", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Person", "IDCard", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Person", "Email", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.Announcement", "PublisherId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Announcement", new[] { "PublisherId" });
            AlterColumn("dbo.Person", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.Person", "IDCard", c => c.String(maxLength: 32));
            AlterColumn("dbo.Announcement", "PublisherId", c => c.String(maxLength: 256));
            AlterColumn("dbo.Announcement", "PublisherId", c => c.String());
            RenameColumn(table: "dbo.Announcement", name: "PublisherId", newName: "Publisher_UserId");
            AddColumn("dbo.Announcement", "PublisherId", c => c.String());
            CreateIndex("dbo.Announcement", "Publisher_UserId");
        }
    }
}
