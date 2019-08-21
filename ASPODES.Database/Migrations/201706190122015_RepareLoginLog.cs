namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepareLoginLog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoginLog", "Token", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoginLog", "Token", c => c.String());
        }
    }
}
