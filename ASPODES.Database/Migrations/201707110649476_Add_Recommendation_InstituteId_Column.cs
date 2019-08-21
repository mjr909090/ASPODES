namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Recommendation_InstituteId_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recommendation", "InstituteId", c => c.Int(nullable: false));
            CreateIndex("dbo.Recommendation", "InstituteId");
            AddForeignKey("dbo.Recommendation", "InstituteId", "dbo.Institute", "InstituteId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recommendation", "InstituteId", "dbo.Institute");
            DropIndex("dbo.Recommendation", new[] { "InstituteId" });
            DropColumn("dbo.Recommendation", "InstituteId");
        }
    }
}
