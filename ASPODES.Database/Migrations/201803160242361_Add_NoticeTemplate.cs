namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NoticeTemplate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notice", "SenderId", "dbo.User");
            DropIndex("dbo.Notice", new[] { "SenderId" });
            CreateTable(
                "dbo.NoticeTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Notice", "NoticeTemplateId", c => c.Int(nullable: false));
            CreateIndex("dbo.Notice", "NoticeTemplateId");
            AddForeignKey("dbo.Notice", "NoticeTemplateId", "dbo.NoticeTemplates", "Id");
            DropColumn("dbo.Notice", "SenderId");
            DropColumn("dbo.Notice", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notice", "Content", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.Notice", "SenderId", c => c.String(nullable: false, maxLength: 256));
            DropForeignKey("dbo.Notice", "NoticeTemplateId", "dbo.NoticeTemplates");
            DropIndex("dbo.Notice", new[] { "NoticeTemplateId" });
            DropColumn("dbo.Notice", "NoticeTemplateId");
            DropTable("dbo.NoticeTemplates");
            CreateIndex("dbo.Notice", "SenderId");
            AddForeignKey("dbo.Notice", "SenderId", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
