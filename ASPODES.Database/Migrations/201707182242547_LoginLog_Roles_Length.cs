namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginLog_Roles_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoginLog", "Roles", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoginLog", "Roles", c => c.String(maxLength: 64));
        }
    }
}
