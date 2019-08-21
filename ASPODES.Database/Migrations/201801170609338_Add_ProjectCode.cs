namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ProjectCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnnualTask", "Name", c => c.String());
            AddColumn("dbo.AnnualTaskDoc", "DisplayName", c => c.String());
            AddColumn("dbo.Project", "ProjectCode", c => c.String());
            AddColumn("dbo.ProjectDoc", "DisplayName", c => c.String());
            AddColumn("dbo.ApplicationDoc", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationDoc", "DisplayName");
            DropColumn("dbo.ProjectDoc", "DisplayName");
            DropColumn("dbo.Project", "ProjectCode");
            DropColumn("dbo.AnnualTaskDoc", "DisplayName");
            DropColumn("dbo.AnnualTask", "Name");
        }
    }
}
