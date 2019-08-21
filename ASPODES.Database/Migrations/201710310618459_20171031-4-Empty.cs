namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201710314Empty : DbMigration
    {
        public override void Up()
        {
            /*
            DropForeignKey("dbo.ApplicationDoc", "Application_ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationField", "Application_ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ReviewAssignment", "Application_ApplicationId", "dbo.Application");
            DropIndex("dbo.ApplicationDoc", new[] { "Application_ApplicationId" });
            DropIndex("dbo.ApplicationField", new[] { "Application_ApplicationId" });
            DropIndex("dbo.ReviewAssignment", new[] { "Application_ApplicationId" });
            DropColumn("dbo.ApplicationDoc", "Application_ApplicationId");
            DropColumn("dbo.ApplicationField", "Application_ApplicationId");
            DropColumn("dbo.ReviewAssignment", "Application_ApplicationId");
             */
        }
        
        public override void Down()
        {
            /*
            AddColumn("dbo.ReviewAssignment", "Application_ApplicationId", c => c.String(maxLength: 64));
            AddColumn("dbo.ApplicationField", "Application_ApplicationId", c => c.String(maxLength: 64));
            AddColumn("dbo.ApplicationDoc", "Application_ApplicationId", c => c.String(maxLength: 64));
            CreateIndex("dbo.ReviewAssignment", "Application_ApplicationId");
            CreateIndex("dbo.ApplicationField", "Application_ApplicationId");
            CreateIndex("dbo.ApplicationDoc", "Application_ApplicationId");
            AddForeignKey("dbo.ReviewAssignment", "Application_ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.ApplicationField", "Application_ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.ApplicationDoc", "Application_ApplicationId", "dbo.Application", "ApplicationId");
        */
          }
    }
}
