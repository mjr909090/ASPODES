namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Del_TemplateReson_FK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TemplateReason", "EditorId", "dbo.User");
            DropIndex("dbo.TemplateReason", new[] { "EditorId" });
            AlterColumn("dbo.TemplateReason", "EditorId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TemplateReason", "EditorId", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.TemplateReason", "EditorId");
            AddForeignKey("dbo.TemplateReason", "EditorId", "dbo.User", "UserId");
        }
    }
}
