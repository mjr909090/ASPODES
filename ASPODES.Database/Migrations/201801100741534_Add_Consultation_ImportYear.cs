namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Consultation_ImportYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationConsultation", "ImportTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProjectConsultation", "ImportTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectConsultation", "ImportTime");
            DropColumn("dbo.ApplicationConsultation", "ImportTime");
        }
    }
}
