namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReviewComment_Amount_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReviewComment", "Amount", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReviewComment", "Amount");
        }
    }
}
