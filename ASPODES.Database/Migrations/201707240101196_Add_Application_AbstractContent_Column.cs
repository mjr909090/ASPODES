namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Application_AbstractContent_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "AbstractContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "AbstractContent");
        }
    }
}
