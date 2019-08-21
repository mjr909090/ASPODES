namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_email : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Email", "SenderId", "dbo.User");
            DropIndex("dbo.Email", new[] { "SenderId" });
            AddColumn("dbo.Email", "IdentifyCode", c => c.String());
            AlterColumn("dbo.Email", "SenderId", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Email", "SenderId", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.Email", "IdentifyCode");
            CreateIndex("dbo.Email", "SenderId");
            AddForeignKey("dbo.Email", "SenderId", "dbo.User", "UserId");
        }
    }
}
