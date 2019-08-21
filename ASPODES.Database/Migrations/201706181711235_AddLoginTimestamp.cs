namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoginTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoginLog", "LoginTimeStamp", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoginLog", "LoginTimeStamp");
        }
    }
}
