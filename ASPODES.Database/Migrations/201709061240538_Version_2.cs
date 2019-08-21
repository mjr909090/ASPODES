namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Announcement", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Announcement", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Announcement", "Content", c => c.String());
            AlterColumn("dbo.Announcement", "Title", c => c.String());
        }
    }
}
