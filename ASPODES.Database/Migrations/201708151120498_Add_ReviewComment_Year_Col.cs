namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReviewComment_Year_Col : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReviewComment", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReviewComment", "Year");
        }
    }
}
