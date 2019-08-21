namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Combine_Person_ExpertInfo_Table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExpertField", "ExpertInfoId", "dbo.ExpertInfo");
            DropIndex("dbo.ExpertField", new[] { "ExpertInfoId" });
            AddColumn("dbo.Person", "EducationHistor", c => c.String());
            AddColumn("dbo.Person", "WorkingHistory", c => c.String());
            AddColumn("dbo.Person", "Achievements", c => c.String(nullable: false, maxLength: 512));
            AddColumn("dbo.ExpertField", "UserId", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.ExpertField", "UserId");
            AddForeignKey("dbo.ExpertField", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            DropColumn("dbo.ExpertField", "ExpertInfoId");
            DropTable("dbo.ExpertInfo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExpertInfo",
                c => new
                    {
                        ExpertInfoId = c.String(nullable: false, maxLength: 256),
                        IDCard = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 64),
                        EducationHistor = c.String(),
                        WorkingHistory = c.String(),
                        Achievements = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.ExpertInfoId);
            
            AddColumn("dbo.ExpertField", "ExpertInfoId", c => c.String(nullable: false, maxLength: 256));
            DropForeignKey("dbo.ExpertField", "UserId", "dbo.User");
            DropIndex("dbo.ExpertField", new[] { "UserId" });
            DropColumn("dbo.ExpertField", "UserId");
            DropColumn("dbo.Person", "Achievements");
            DropColumn("dbo.Person", "WorkingHistory");
            DropColumn("dbo.Person", "EducationHistor");
            CreateIndex("dbo.ExpertField", "ExpertInfoId");
            AddForeignKey("dbo.ExpertField", "ExpertInfoId", "dbo.ExpertInfo", "ExpertInfoId", cascadeDelete: true);
        }
    }
}
