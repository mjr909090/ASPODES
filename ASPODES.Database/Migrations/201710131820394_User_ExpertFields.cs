namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_ExpertFields : DbMigration
    {
        public override void Up()
        {
            /*
            AddColumn("dbo.ExpertField", "User_UserId", c => c.String(maxLength: 256));
            CreateIndex("dbo.ExpertField", "User_UserId");
            AddForeignKey("dbo.ExpertField", "User_UserId", "dbo.User", "UserId");
            */
        }
        
        public override void Down()
        {
             /*
            DropForeignKey("dbo.ExpertField", "User_UserId", "dbo.User");
            DropIndex("dbo.ExpertField", new[] { "User_UserId" });
            DropColumn("dbo.ExpertField", "User_UserId");
              */ 
        }
    }
}
