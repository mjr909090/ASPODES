namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201710312Empty : DbMigration
    {
        public override void Up()
        {
            /*
            AddColumn("dbo.ApplicationDoc", "Application_ApplicationId", c => c.String(maxLength: 64));
            AddColumn("dbo.ApplicationField", "Application_ApplicationId", c => c.String(maxLength: 64));
            AddColumn("dbo.ReviewAssignment", "Application_ApplicationId", c => c.String(maxLength: 64));
            CreateIndex("dbo.ApplicationDoc", "Application_ApplicationId");
            CreateIndex("dbo.ApplicationField", "Application_ApplicationId");
            CreateIndex("dbo.ReviewAssignment", "Application_ApplicationId");
            AddForeignKey("dbo.ApplicationDoc", "Application_ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.ApplicationField", "Application_ApplicationId", "dbo.Application", "ApplicationId");
            AddForeignKey("dbo.ReviewAssignment", "Application_ApplicationId", "dbo.Application", "ApplicationId");
        
             */ }
        
        public override void Down()
        {
            /*
            DropForeignKey("dbo.ReviewAssignment", "Application_ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationField", "Application_ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationDoc", "Application_ApplicationId", "dbo.Application");
            DropIndex("dbo.ReviewAssignment", new[] { "Application_ApplicationId" });
            DropIndex("dbo.ApplicationField", new[] { "Application_ApplicationId" });
            DropIndex("dbo.ApplicationDoc", new[] { "Application_ApplicationId" });
            DropColumn("dbo.ReviewAssignment", "Application_ApplicationId");
            DropColumn("dbo.ApplicationField", "Application_ApplicationId");
            DropColumn("dbo.ApplicationDoc", "Application_ApplicationId");
        */
              }
    }
}
