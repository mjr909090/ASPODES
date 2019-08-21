namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SubjectColumn_Email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Email", "Subject", c => c.String());
            AlterColumn("dbo.Email", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Email", "Status", c => c.String(maxLength: 64));
            DropColumn("dbo.Email", "Subject");
        }
    }
}
