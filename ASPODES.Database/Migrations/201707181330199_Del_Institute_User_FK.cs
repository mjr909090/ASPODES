namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Del_Institute_User_FK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Institute", "Contact_UserId", "dbo.User");
            DropIndex("dbo.Institute", new[] { "Contact_UserId" });
            DropColumn("dbo.Institute", "Contact_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Institute", "Contact_UserId", c => c.String(maxLength: 256));
            CreateIndex("dbo.Institute", "Contact_UserId");
            AddForeignKey("dbo.Institute", "Contact_UserId", "dbo.User", "UserId");
        }
    }
}
