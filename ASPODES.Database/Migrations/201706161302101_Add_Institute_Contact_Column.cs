namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Institute_Contact_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Institute", "ComtactId", c => c.String(maxLength: 256));
            CreateIndex("dbo.Institute", "ComtactId");
            AddForeignKey("dbo.Institute", "ComtactId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Institute", "ComtactId", "dbo.User");
            DropIndex("dbo.Institute", new[] { "ComtactId" });
            DropColumn("dbo.Institute", "ComtactId");
        }
    }
}
