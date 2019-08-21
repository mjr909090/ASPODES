namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Person : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.User", new[] { "PersonId" });
            AlterColumn("dbo.User", "PersonId", c => c.Int());
            CreateIndex("dbo.User", "PersonId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.User", new[] { "PersonId" });
            AlterColumn("dbo.User", "PersonId", c => c.Int(nullable: false));
            CreateIndex("dbo.User", "PersonId");
        }
    }
}
