namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Primary_Consultation : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ApplicationConsultation");
            DropPrimaryKey("dbo.ProjectConsultation");
            AlterColumn("dbo.ApplicationConsultation", "ConsultationId", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.ProjectConsultation", "ConsultationId", c => c.String(nullable: false, maxLength: 64));
            AddPrimaryKey("dbo.ApplicationConsultation", "ConsultationId");
            AddPrimaryKey("dbo.ProjectConsultation", "ConsultationId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProjectConsultation");
            DropPrimaryKey("dbo.ApplicationConsultation");
            AlterColumn("dbo.ProjectConsultation", "ConsultationId", c => c.Int(nullable: false));
            AlterColumn("dbo.ApplicationConsultation", "ConsultationId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ProjectConsultation", "ConsultationId");
            AddPrimaryKey("dbo.ApplicationConsultation", "ConsultationId");
        }
    }
}
