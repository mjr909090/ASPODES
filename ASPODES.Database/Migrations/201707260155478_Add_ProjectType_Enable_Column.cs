namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ProjectType_Enable_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectTypes", "Enable", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectTypes", "Enable");
        }
    }
}
