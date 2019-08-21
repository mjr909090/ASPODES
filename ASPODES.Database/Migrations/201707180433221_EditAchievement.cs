namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditAchievement : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "Achievements", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Achievements", c => c.String(maxLength: 512));
        }
    }
}
