namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoginToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoginLog", "Token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoginLog", "Token");
        }
    }
}
