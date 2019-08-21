namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AnnualTask_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnualTaskBudgetItem",
                c => new
                    {
                        AnnualTaskBudgetItemId = c.Int(nullable: false, identity: true),
                        AnnualTaskId = c.Int(nullable: false),
                        SubjectId = c.String(nullable: false, maxLength: 30),
                        Amount = c.Double(nullable: false),
                        Reason = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.AnnualTaskBudgetItemId)
                .ForeignKey("dbo.AnnualTask", t => t.AnnualTaskId, cascadeDelete: true)
                .ForeignKey("dbo.AccountingSubject", t => t.SubjectId)
                .Index(t => t.AnnualTaskId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.AnnualTask",
                c => new
                    {
                        AnnualTaskId = c.Int(nullable: false, identity: true),
                        ProjectId = c.String(nullable: false, maxLength: 64),
                        LeaderId = c.Int(nullable: false),
                        InstituteId = c.Int(nullable: false),
                        CurrentBudget = c.Double(),
                        Year = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnualTaskId)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .ForeignKey("dbo.Person", t => t.LeaderId)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.LeaderId)
                .Index(t => t.InstituteId);
            
            CreateTable(
                "dbo.AnnualTaskDoc",
                c => new
                    {
                        AnnualTaskDocId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(nullable: false),
                        RelativeURL = c.String(nullable: false, maxLength: 256),
                        AnnualTaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnnualTaskDocId)
                .ForeignKey("dbo.AnnualTask", t => t.AnnualTaskId, cascadeDelete: true)
                .Index(t => t.AnnualTaskId);
            
            CreateTable(
                "dbo.AnnualTaskInstBudget",
                c => new
                    {
                        AnnualTaskId = c.Int(nullable: false),
                        InstituteId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnnualTaskId, t.InstituteId })
                .ForeignKey("dbo.AnnualTask", t => t.AnnualTaskId, cascadeDelete: true)
                .ForeignKey("dbo.Institute", t => t.InstituteId)
                .Index(t => t.AnnualTaskId)
                .Index(t => t.InstituteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnnualTaskInstBudget", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.AnnualTaskInstBudget", "AnnualTaskId", "dbo.AnnualTask");
            DropForeignKey("dbo.AnnualTaskDoc", "AnnualTaskId", "dbo.AnnualTask");
            DropForeignKey("dbo.AnnualTaskBudgetItem", "SubjectId", "dbo.AccountingSubject");
            DropForeignKey("dbo.AnnualTaskBudgetItem", "AnnualTaskId", "dbo.AnnualTask");
            DropForeignKey("dbo.AnnualTask", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.AnnualTask", "LeaderId", "dbo.Person");
            DropForeignKey("dbo.AnnualTask", "InstituteId", "dbo.Institute");
            DropIndex("dbo.AnnualTaskInstBudget", new[] { "InstituteId" });
            DropIndex("dbo.AnnualTaskInstBudget", new[] { "AnnualTaskId" });
            DropIndex("dbo.AnnualTaskDoc", new[] { "AnnualTaskId" });
            DropIndex("dbo.AnnualTask", new[] { "InstituteId" });
            DropIndex("dbo.AnnualTask", new[] { "LeaderId" });
            DropIndex("dbo.AnnualTask", new[] { "ProjectId" });
            DropIndex("dbo.AnnualTaskBudgetItem", new[] { "SubjectId" });
            DropIndex("dbo.AnnualTaskBudgetItem", new[] { "AnnualTaskId" });
            DropTable("dbo.AnnualTaskInstBudget");
            DropTable("dbo.AnnualTaskDoc");
            DropTable("dbo.AnnualTask");
            DropTable("dbo.AnnualTaskBudgetItem");
        }
    }
}
