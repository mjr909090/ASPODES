namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ProjectLog_Table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectDoc", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectMember", "ProjectId", "dbo.Project");
            DropIndex("dbo.ProjectDoc", new[] { "ProjectId" });
            DropIndex("dbo.ProjectMember", new[] { "ProjectId" });
            
            AlterColumn("dbo.ProjectDoc", "ProjectId", c => c.String(nullable: false, maxLength: 64));


            DropTable("dbo.Project");
            CreateTable(
                "dbo.Project",
                c => new
                {
                    ProjectId = c.String(nullable: false, unicode: true, maxLength: 64),
                    ApplicationId = c.String(nullable: false, maxLength: 64),
                    Name = c.String(maxLength: 256),
                    LeaderId = c.Int(nullable: false),
                    InstituteId = c.Int(nullable: false),
                    Period = c.Int(nullable: false, defaultValue: 1),
                    StartDate = c.DateTime(nullable: true),
                    EndDate = c.DateTime(),
                    TotalBudget = c.Double(nullable: true, defaultValue: 0),
                    ProjectTypeId = c.Int(nullable: false),
                    DelegateType = c.Int(nullable: true, defaultValue: 0),
                    Status = c.Int(nullable: false, defaultValue: 0)
                })
                .PrimaryKey(p => p.ProjectId)
                .ForeignKey("dbo.Application", p => p.ApplicationId)
                .ForeignKey("dbo.Person", p => p.LeaderId)
                .ForeignKey("dbo.Institute", p => p.InstituteId)
                .ForeignKey("dbo.ProjectTypes", p => p.ProjectTypeId)
                .Index(p => p.ProjectId);

            DropTable("dbo.ProjectMember");
            CreateTable("dbo.ProjectMember", c => new
            {
                ProjectId = c.String(maxLength: 64, nullable: false),
                PersonId = c.Int(nullable: false),
                Task = c.String(maxLength: 1024)
            })
            .PrimaryKey(pm => new { pm.ProjectId, pm.PersonId})
            .ForeignKey("dbo.Project", pm=>pm.ProjectId)
            .ForeignKey("dbo.Person", pm=>pm.PersonId)
            .Index( pm=>pm.ProjectId );

            CreateIndex("dbo.ProjectDoc", "ProjectId");
            AddForeignKey("dbo.ProjectDoc", "ProjectId", "dbo.Project", "ProjectId", cascadeDelete: true);

            CreateTable(
                "dbo.ProjectLog",
                c => new
                {
                    ProjectLogId = c.Int(nullable: false, identity: true),
                    ProjectId = c.String(nullable: false, maxLength: 64),
                    UserId = c.String(),
                    AssociateId = c.Int(nullable: false),
                    AssociateType = c.Int(nullable: false),
                    OperationType = c.Int(nullable: false),
                    Comment = c.String(maxLength: 124),
                })
                .PrimaryKey(t => t.ProjectLogId)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.ProjectId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectMember", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectDoc", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectLog", "ProjectId", "dbo.Project");
            DropIndex("dbo.ProjectMember", new[] { "ProjectId" });
            DropIndex("dbo.ProjectLog", new[] { "ProjectId" });
            DropIndex("dbo.Project", new[] { "ProjectId" });
            DropIndex("dbo.ProjectDoc", new[] { "ProjectId" });
            DropPrimaryKey("dbo.ProjectMember");
            DropPrimaryKey("dbo.Project");
            AlterColumn("dbo.ProjectMember", "ProjectId", c => c.Int(nullable: false));
            AlterColumn("dbo.Project", "ProjectId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ProjectDoc", "ProjectId", c => c.Int(nullable: false));
            DropTable("dbo.ProjectLog");
            AddPrimaryKey("dbo.ProjectMember", new[] { "ProjectId", "PersonId" });
            AddPrimaryKey("dbo.Project", "ProjectId");
            RenameColumn(table: "dbo.Project", name: "ProjectId", newName: "Application_ApplicationId");
            AddColumn("dbo.Project", "ProjectId", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.ProjectMember", "ProjectId");
            CreateIndex("dbo.Project", "Application_ApplicationId");
            CreateIndex("dbo.ProjectDoc", "ProjectId");
            AddForeignKey("dbo.ProjectMember", "ProjectId", "dbo.Project", "ProjectId", cascadeDelete: true);
            AddForeignKey("dbo.ProjectDoc", "ProjectId", "dbo.Project", "ProjectId", cascadeDelete: true);
        }
    }
}
