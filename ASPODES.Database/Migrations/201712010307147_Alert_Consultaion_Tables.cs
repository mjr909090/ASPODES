namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alert_Consultaion_Tables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationConsultation", "Institude_InstituteId", "dbo.Institute");
            DropForeignKey("dbo.ApplicationConsultation", "Leader_PersonId", "dbo.Person");
            DropForeignKey("dbo.ApplicationConsultation", "ProjectType_ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ProjectConsultation", "Institude_InstituteId", "dbo.Institute");
            DropForeignKey("dbo.ProjectConsultation", "Leader_PersonId", "dbo.Person");
            DropForeignKey("dbo.ProjectConsultation", "ProjectType_ProjectTypeId", "dbo.ProjectTypes");
            DropIndex("dbo.ApplicationConsultation", new[] { "Institude_InstituteId" });
            DropIndex("dbo.ApplicationConsultation", new[] { "Leader_PersonId" });
            DropIndex("dbo.ApplicationConsultation", new[] { "ProjectType_ProjectTypeId" });
            DropIndex("dbo.ProjectConsultation", new[] { "Institude_InstituteId" });
            DropIndex("dbo.ProjectConsultation", new[] { "Leader_PersonId" });
            DropIndex("dbo.ProjectConsultation", new[] { "ProjectType_ProjectTypeId" });
            DropColumn("dbo.ApplicationConsultation", "Institude_InstituteId");
            DropColumn("dbo.ApplicationConsultation", "Leader_PersonId");
            DropColumn("dbo.ApplicationConsultation", "ProjectType_ProjectTypeId");
            DropColumn("dbo.ApplicationConsultation", "LeaderId");
            DropColumn("dbo.ApplicationConsultation", "InstituteId");
            DropColumn("dbo.ApplicationConsultation", "ProjectTypeId");
            DropColumn("dbo.ProjectConsultation", "Institude_InstituteId");
            DropColumn("dbo.ProjectConsultation", "Leader_PersonId");
            DropColumn("dbo.ProjectConsultation", "ProjectType_ProjectTypeId");
            DropColumn("dbo.ProjectConsultation", "LeaderId");
            DropColumn("dbo.ProjectConsultation", "InstituteId");
            DropColumn("dbo.ProjectConsultation", "ProjectTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectConsultation", "ProjectTypeId", c => c.Int());
            AddColumn("dbo.ProjectConsultation", "InstituteId", c => c.Int());
            AddColumn("dbo.ProjectConsultation", "LeaderId", c => c.Int());
            AddColumn("dbo.ProjectConsultation", "ProjectType_ProjectTypeId", c => c.Int());
            AddColumn("dbo.ProjectConsultation", "Leader_PersonId", c => c.Int());
            AddColumn("dbo.ProjectConsultation", "Institude_InstituteId", c => c.Int());
            AddColumn("dbo.ApplicationConsultation", "ProjectTypeId", c => c.Int());
            AddColumn("dbo.ApplicationConsultation", "InstituteId", c => c.Int());
            AddColumn("dbo.ApplicationConsultation", "LeaderId", c => c.Int());
            AddColumn("dbo.ApplicationConsultation", "ProjectType_ProjectTypeId", c => c.Int());
            AddColumn("dbo.ApplicationConsultation", "Leader_PersonId", c => c.Int());
            AddColumn("dbo.ApplicationConsultation", "Institude_InstituteId", c => c.Int());
            CreateIndex("dbo.ProjectConsultation", "ProjectType_ProjectTypeId");
            CreateIndex("dbo.ProjectConsultation", "Leader_PersonId");
            CreateIndex("dbo.ProjectConsultation", "Institude_InstituteId");
            CreateIndex("dbo.ApplicationConsultation", "ProjectType_ProjectTypeId");
            CreateIndex("dbo.ApplicationConsultation", "Leader_PersonId");
            CreateIndex("dbo.ApplicationConsultation", "Institude_InstituteId");
            AddForeignKey("dbo.ProjectConsultation", "ProjectType_ProjectTypeId", "dbo.ProjectTypes", "ProjectTypeId");
            AddForeignKey("dbo.ProjectConsultation", "Leader_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ProjectConsultation", "Institude_InstituteId", "dbo.Institute", "InstituteId");
            AddForeignKey("dbo.ApplicationConsultation", "ProjectType_ProjectTypeId", "dbo.ProjectTypes", "ProjectTypeId");
            AddForeignKey("dbo.ApplicationConsultation", "Leader_PersonId", "dbo.Person", "PersonId");
            AddForeignKey("dbo.ApplicationConsultation", "Institude_InstituteId", "dbo.Institute", "InstituteId");
        }
    }
}
