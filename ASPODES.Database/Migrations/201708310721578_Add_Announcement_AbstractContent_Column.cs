namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Announcement_AbstractContent_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcement", "AbstractContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcement", "AbstractContent");
        }
    }
}
