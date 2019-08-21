namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Project_Commnet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnnualTask", "Comment", c => c.String());
            AddColumn("dbo.Project", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Comment");
            DropColumn("dbo.AnnualTask", "Comment");
        }
    }
}
