namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allowExpertInfoNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExpertInfo", "EducationHistor", c => c.String());
            AlterColumn("dbo.ExpertInfo", "WorkingHistory", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExpertInfo", "WorkingHistory", c => c.String(nullable: false));
            AlterColumn("dbo.ExpertInfo", "EducationHistor", c => c.String(nullable: false));
        }
    }
}
