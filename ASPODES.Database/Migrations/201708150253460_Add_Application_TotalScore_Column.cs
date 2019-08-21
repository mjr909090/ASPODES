namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Application_TotalScore_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "TotalScore", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "TotalScore");
        }
    }
}
