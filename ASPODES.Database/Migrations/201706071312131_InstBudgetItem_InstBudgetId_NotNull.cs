namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstBudgetItem_InstBudgetId_NotNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InstAnnualBudget", "InstBudgetId", "dbo.InstBudget");
            DropIndex("dbo.InstAnnualBudget", new[] { "InstBudgetId" });
            AlterColumn("dbo.InstAnnualBudget", "InstBudgetId", c => c.Int(nullable: false));
            CreateIndex("dbo.InstAnnualBudget", "InstBudgetId");
            AddForeignKey("dbo.InstAnnualBudget", "InstBudgetId", "dbo.InstBudget", "InstBudgetId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstAnnualBudget", "InstBudgetId", "dbo.InstBudget");
            DropIndex("dbo.InstAnnualBudget", new[] { "InstBudgetId" });
            AlterColumn("dbo.InstAnnualBudget", "InstBudgetId", c => c.Int());
            CreateIndex("dbo.InstAnnualBudget", "InstBudgetId");
            AddForeignKey("dbo.InstAnnualBudget", "InstBudgetId", "dbo.InstBudget", "InstBudgetId");
        }
    }
}
