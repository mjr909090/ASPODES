namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Del_ReviewAssignment_PDF_Column : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReviewAssignment", "PDFId", "dbo.ApplicationDoc");
            DropIndex("dbo.ReviewAssignment", new[] { "PDFId" });
            DropColumn("dbo.ReviewAssignment", "PDFId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReviewAssignment", "PDFId", c => c.Int(nullable: false));
            CreateIndex("dbo.ReviewAssignment", "PDFId");
            AddForeignKey("dbo.ReviewAssignment", "PDFId", "dbo.ApplicationDoc", "ApplicationDocId");
        }
    }
}
