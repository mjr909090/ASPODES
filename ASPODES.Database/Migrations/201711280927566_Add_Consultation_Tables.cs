namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Consultation_Tables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Consultation", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.Consultation", "InstituteId", "dbo.Institute");
            DropForeignKey("dbo.Consultation", "LeaderId", "dbo.Person");
            DropForeignKey("dbo.Consultation", "ProjectTypeId", "dbo.ProjectTypes");
            DropIndex("dbo.Consultation", new[] { "ApplicationId" });
            DropIndex("dbo.Consultation", new[] { "LeaderId" });
            DropIndex("dbo.Consultation", new[] { "InstituteId" });
            DropIndex("dbo.Consultation", new[] { "ProjectTypeId" });
            CreateTable(
                "dbo.ApplicationConsultation",
                c => new
                    {
                        ConsultationId = c.Int(nullable: false),
                        Institude_InstituteId = c.Int(),
                        Leader_PersonId = c.Int(),
                        ProjectType_ProjectTypeId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 256),
                        Score = c.Double(nullable: false),
                        LeaderId = c.Int(),
                        InstituteId = c.Int(),
                        Period = c.Int(nullable: false),
                        Budget = c.Double(nullable: false),
                        CurrentYearBudget = c.Double(nullable: false),
                        ProjectTypeId = c.Int(),
                        DelegateType = c.Int(nullable: false),
                        Opinion = c.String(maxLength: 1024),
                        ApplicationId = c.String(maxLength: 64),
                        Result = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConsultationId)
                .ForeignKey("dbo.Institute", t => t.Institude_InstituteId)
                .ForeignKey("dbo.Person", t => t.Leader_PersonId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectType_ProjectTypeId)
                .ForeignKey("dbo.Application", t => t.ApplicationId)
                .Index(t => t.Institude_InstituteId)
                .Index(t => t.Leader_PersonId)
                .Index(t => t.ProjectType_ProjectTypeId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ProjectConsultation",
                c => new
                    {
                        ConsultationId = c.Int(nullable: false),
                        Institude_InstituteId = c.Int(),
                        Leader_PersonId = c.Int(),
                        ProjectType_ProjectTypeId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 256),
                        Score = c.Double(nullable: false),
                        LeaderId = c.Int(),
                        InstituteId = c.Int(),
                        Period = c.Int(nullable: false),
                        Budget = c.Double(nullable: false),
                        CurrentYearBudget = c.Double(nullable: false),
                        ProjectTypeId = c.Int(),
                        DelegateType = c.Int(nullable: false),
                        Opinion = c.String(maxLength: 1024),
                        ArrivalBudget = c.Double(),
                        Result = c.Int(nullable: false),
                        ProjectId = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.ConsultationId)
                .ForeignKey("dbo.Institute", t => t.Institude_InstituteId)
                .ForeignKey("dbo.Person", t => t.Leader_PersonId)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectType_ProjectTypeId)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.Institude_InstituteId)
                .Index(t => t.Leader_PersonId)
                .Index(t => t.ProjectType_ProjectTypeId)
                .Index(t => t.ProjectId);
            
            DropTable("dbo.Consultation");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Consultation",
                c => new
                    {
                        ConsultationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Score = c.Double(nullable: false),
                        ApplicationId = c.String(nullable: false, maxLength: 64),
                        LeaderId = c.Int(nullable: false),
                        InstituteId = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Budget = c.Double(nullable: false),
                        FirstYearBudget = c.Double(nullable: false),
                        ProjectTypeId = c.Int(nullable: false),
                        DelegateType = c.Int(nullable: false),
                        Result = c.Int(nullable: false),
                        Opinion = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.ConsultationId);
            
            DropForeignKey("dbo.ProjectConsultation", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectConsultation", "ProjectType_ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ProjectConsultation", "Leader_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProjectConsultation", "Institude_InstituteId", "dbo.Institute");
            DropForeignKey("dbo.ApplicationConsultation", "ApplicationId", "dbo.Application");
            DropForeignKey("dbo.ApplicationConsultation", "ProjectType_ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ApplicationConsultation", "Leader_PersonId", "dbo.Person");
            DropForeignKey("dbo.ApplicationConsultation", "Institude_InstituteId", "dbo.Institute");
            DropIndex("dbo.ProjectConsultation", new[] { "ProjectId" });
            DropIndex("dbo.ProjectConsultation", new[] { "ProjectType_ProjectTypeId" });
            DropIndex("dbo.ProjectConsultation", new[] { "Leader_PersonId" });
            DropIndex("dbo.ProjectConsultation", new[] { "Institude_InstituteId" });
            DropIndex("dbo.ApplicationConsultation", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationConsultation", new[] { "ProjectType_ProjectTypeId" });
            DropIndex("dbo.ApplicationConsultation", new[] { "Leader_PersonId" });
            DropIndex("dbo.ApplicationConsultation", new[] { "Institude_InstituteId" });
            DropTable("dbo.ProjectConsultation");
            DropTable("dbo.ApplicationConsultation");
            CreateIndex("dbo.Consultation", "ProjectTypeId");
            CreateIndex("dbo.Consultation", "InstituteId");
            CreateIndex("dbo.Consultation", "LeaderId");
            CreateIndex("dbo.Consultation", "ApplicationId");
            AddForeignKey("dbo.Consultation", "ProjectTypeId", "dbo.ProjectTypes", "ProjectTypeId");
            AddForeignKey("dbo.Consultation", "LeaderId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.Consultation", "InstituteId", "dbo.Institute", "InstituteId");
            AddForeignKey("dbo.Consultation", "ApplicationId", "dbo.Application", "ApplicationId", cascadeDelete: true);
        }
    }
}
