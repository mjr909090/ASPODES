namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvisitlogaddRequestUri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitLog", "RequestUri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitLog", "RequestUri");
        }
    }
}
