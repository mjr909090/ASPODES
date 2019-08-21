namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Institute_Contact : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Institute", "ContactId", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.Institute", "ContactId");
            AddForeignKey("dbo.Institute", "ContactId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Institute", "ContactId", "dbo.User");
            DropIndex("dbo.Institute", new[] { "ContactId" });
            AlterColumn("dbo.Institute", "ContactId", c => c.String());
        }
    }
}
