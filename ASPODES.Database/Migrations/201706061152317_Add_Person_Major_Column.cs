namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Person_Major_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "Major", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "Major");
        }
    }
}
