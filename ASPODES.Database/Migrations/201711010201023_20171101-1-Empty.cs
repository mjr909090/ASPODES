namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201711011Empty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "IDCard", c => c.String(maxLength: 32));
            AlterColumn("dbo.Person", "Email", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Person", "IDCard", c => c.String(nullable: false, maxLength: 32));
        }
    }
}
