namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Del_Column_Consultation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationConsultation", "Name");
            DropColumn("dbo.ApplicationConsultation", "Score");
            DropColumn("dbo.ApplicationConsultation", "Opinion");
            DropColumn("dbo.ProjectConsultation", "Name");
            DropColumn("dbo.ProjectConsultation", "Score");
            DropColumn("dbo.ProjectConsultation", "Opinion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectConsultation", "Opinion", c => c.String(maxLength: 1024));
            AddColumn("dbo.ProjectConsultation", "Score", c => c.Double(nullable: false));
            AddColumn("dbo.ProjectConsultation", "Name", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.ApplicationConsultation", "Opinion", c => c.String(maxLength: 1024));
            AddColumn("dbo.ApplicationConsultation", "Score", c => c.Double(nullable: false));
            AddColumn("dbo.ApplicationConsultation", "Name", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
