namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Column_Person_Achivment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "Achievements", c => c.String(maxLength: 512));
            AlterColumn("dbo.Person", "Status", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Status", c => c.String(maxLength: 4));
            AlterColumn("dbo.Person", "Achievements", c => c.String(nullable: false, maxLength: 512));
        }
    }
}
