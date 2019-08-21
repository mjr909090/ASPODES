namespace ASPODES.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changenoticetype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notice", "NoticeType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notice", "NoticeType", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
