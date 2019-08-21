namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ApplicationAssignment_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationAssignment",
                c => new
                    {
                        ApplicationAssignmentId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 256),
                        ProjectTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicationAssignmentId)
                .ForeignKey("dbo.Authorize", t => new { t.RoleId, t.UserId }, cascadeDelete: true)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId, cascadeDelete: true)
                .Index(t => new { t.RoleId, t.UserId })
                .Index(t => t.ProjectTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationAssignment", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ApplicationAssignment", new[] { "RoleId", "UserId" }, "dbo.Authorize");
            DropIndex("dbo.ApplicationAssignment", new[] { "ProjectTypeId" });
            DropIndex("dbo.ApplicationAssignment", new[] { "RoleId", "UserId" });
            DropTable("dbo.ApplicationAssignment");
        }
    }
}
