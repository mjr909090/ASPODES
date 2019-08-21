namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Project_AnnualTask_Tables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "TotalBudget", c => c.Double(nullable: false));
            DropColumn("dbo.Project", "Budget");
            DropColumn("dbo.Project", "FirstYearBudget");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "FirstYearBudget", c => c.Double(nullable: false));
            AddColumn("dbo.Project", "Budget", c => c.Double(nullable: false));
            DropColumn("dbo.Project", "TotalBudget");
        }
    }
}
