namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Application_EditTime_Notice_Read_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "EditTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notice", "Read", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notice", "Read");
            DropColumn("dbo.Application", "EditTime");
        }
    }
}
