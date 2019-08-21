namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Person_Status_Name_Nullable : DbMigration
    {
        public override void Up()
        {
            /*
            AlterColumn("dbo.Person", "Name", c => c.String());
            AlterColumn("dbo.Person", "FirstName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Person", "LastName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Person", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Person", "Status", c => c.String(maxLength: 4));
             * */
        }
        
        public override void Down()
        {
            /*
            AlterColumn("dbo.Person", "Status", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.Person", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.Person", "LastName", c => c.String(maxLength: 64));
            AlterColumn("dbo.Person", "FirstName", c => c.String(maxLength: 64));
            AlterColumn("dbo.Person", "Name", c => c.String(nullable: false, maxLength: 64));
             * */
        }
    }
}
