namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Institute_Columns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Institute", "Address", c => c.String());
            AddColumn("dbo.Institute", "Zip", c => c.String(maxLength: 32));
            AddColumn("dbo.Institute", "WebSite", c => c.String());
            AddColumn("dbo.Institute", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Institute", "Comment");
            DropColumn("dbo.Institute", "WebSite");
            DropColumn("dbo.Institute", "Zip");
            DropColumn("dbo.Institute", "Address");
        }
    }
}
