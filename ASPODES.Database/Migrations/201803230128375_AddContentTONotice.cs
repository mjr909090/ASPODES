namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentTONotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notice", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notice", "Content");
        }
    }
}
