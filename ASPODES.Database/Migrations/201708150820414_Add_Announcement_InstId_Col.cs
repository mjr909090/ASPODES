namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Announcement_InstId_Col : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcement", "InstituteId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcement", "InstituteId");
        }
    }
}
