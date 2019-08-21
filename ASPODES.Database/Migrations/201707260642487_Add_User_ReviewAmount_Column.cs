namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_User_ReviewAmount_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "ReviewAmount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "ReviewAmount");
        }
    }
}
