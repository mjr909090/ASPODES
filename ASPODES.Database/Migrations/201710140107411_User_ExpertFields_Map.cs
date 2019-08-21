namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_ExpertFields_Map : DbMigration
    {
        public override void Up()
        {
            /*
            DropForeignKey("dbo.ExpertField", "User_UserId", "dbo.User");
            DropIndex("dbo.ExpertField", new[] { "UserId" });
            DropIndex("dbo.ExpertField", new[] { "User_UserId" });
            DropColumn("dbo.ExpertField", "UserId");
            RenameColumn(table: "dbo.ExpertField", name: "User_UserId", newName: "UserId");
            AlterColumn("dbo.ExpertField", "UserId", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.ExpertField", "UserId");
            AddForeignKey("dbo.ExpertField", "UserId", "dbo.User", "UserId", cascadeDelete: true);
             */
        }
        
        public override void Down()
        {
             /*
            DropForeignKey("dbo.ExpertField", "UserId", "dbo.User");
            DropIndex("dbo.ExpertField", new[] { "UserId" });
            AlterColumn("dbo.ExpertField", "UserId", c => c.String(maxLength: 256));
            RenameColumn(table: "dbo.ExpertField", name: "UserId", newName: "User_UserId");
            AddColumn("dbo.ExpertField", "UserId", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.ExpertField", "User_UserId");
            CreateIndex("dbo.ExpertField", "UserId");
            AddForeignKey("dbo.ExpertField", "User_UserId", "dbo.User", "UserId");
              */
        }
    }
}
