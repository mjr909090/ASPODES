namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editReasonTemplate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ReviewComment", new[] { "ReviewCommentId" });
            DropPrimaryKey("dbo.ReviewComment");
            AddColumn("dbo.TemplateReason", "EditTime", c => c.DateTime());
            AlterColumn("dbo.ReviewComment", "ReviewCommentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ReviewComment", "ReviewCommentId");
            CreateIndex("dbo.ReviewComment", "ReviewCommentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReviewComment", new[] { "ReviewCommentId" });
            DropPrimaryKey("dbo.ReviewComment");
            AlterColumn("dbo.ReviewComment", "ReviewCommentId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.TemplateReason", "EditTime");
            AddPrimaryKey("dbo.ReviewComment", "ReviewCommentId");
            CreateIndex("dbo.ReviewComment", "ReviewCommentId");
        }
    }
}
