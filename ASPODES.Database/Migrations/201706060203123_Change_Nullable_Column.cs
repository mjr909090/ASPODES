namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Nullable_Column : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "Birthday", c => c.DateTime());
            AlterColumn("dbo.Institute", "Code", c => c.String(maxLength: 64));
            AlterColumn("dbo.ProjectTypes", "Limit", c => c.Int());
            AlterColumn("dbo.User", "LastLogin", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "LastLogin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProjectTypes", "Limit", c => c.Int(nullable: false));
            AlterColumn("dbo.Institute", "Code", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Person", "Birthday", c => c.DateTime(nullable: false));
        }
    }
}
