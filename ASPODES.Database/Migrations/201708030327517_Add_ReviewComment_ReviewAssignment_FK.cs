namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReviewComment_ReviewAssignment_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReviewComment", "ReviewAssignmentId", c => c.Int(nullable: false));
            AddForeignKey("dbo.ReviewComment", "ReviewCommentId", "dbo.ReviewAssignment", "ReviewAssignmentId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewComment", "ReviewCommentId", "dbo.ReviewAssignment");
            DropIndex("dbo.ReviewComment", new[] { "ReviewCommentId" });
            DropPrimaryKey("dbo.ReviewComment");
            AlterColumn("dbo.ReviewComment", "ReviewCommentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ReviewComment", "ReviewCommentId");
            RenameColumn(table: "dbo.ReviewComment", name: "ReviewCommentId", newName: "ReviewAssignmentId");
            AddColumn("dbo.ReviewComment", "ReviewCommentId", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.ReviewComment", "ReviewAssignmentId");
            AddForeignKey("dbo.ReviewComment", "ReviewAssignmentId", "dbo.ReviewAssignment", "ReviewAssignmentId");
        }
    }
}
